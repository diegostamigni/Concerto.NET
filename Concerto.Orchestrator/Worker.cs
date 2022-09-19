using System.Reflection;
using Concerto.Coordinator.Contracts;
using Concerto.Orchestrator.Settings;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Concerto.Orchestrator;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> logger;
	private readonly IBus bus;
	private readonly ICoordinatorResolver coordinatorResolver;

	private readonly List<(Type EventType, Type EventResult)> supportedEventsWithResults = new();

	public Worker(
		ILogger<Worker> logger,
		IBus bus,
		ICoordinatorResolver coordinatorResolver,
		EventSettings eventSettings)
	{
		this.logger = logger;
		this.bus = bus;
		this.coordinatorResolver = coordinatorResolver;

		foreach (var eventSetting in eventSettings.SupportedEventsWithResult ?? new())
		{
			var assembly = Assembly.GetExecutingAssembly();
			if (!string.IsNullOrEmpty(eventSetting.EventAssemblyName))
			{
				assembly = Assembly.LoadFrom(eventSetting.EventAssemblyName);
			}

			var inputType = assembly.GetType(eventSetting.EventTypeName);
			var outputType = assembly.GetType(eventSetting.EventTypeResult);

			this.supportedEventsWithResults.Add(new(inputType!, outputType!));
		}
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var registerMethod = GetType()
			.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
			.Single(x => x.Name == nameof(RegisterAsync));

		foreach (var (eventType, resultType) in this.supportedEventsWithResults)
		{
			var registerMethodImpl = registerMethod.MakeGenericMethod(eventType, resultType);
			if (registerMethodImpl.Invoke(this, new object[] { stoppingToken }) is not Task task)
			{
				continue;
			}

			await task;
		}
	}

	private Task RegisterAsync<TEvent, TResult>(CancellationToken token)
		=> this.bus.Rpc.RespondAsync<TEvent, TResult>(request => ExecuteAsync<TEvent, TResult>(request, token), token);

	private Task<TResult> ExecuteAsync<TEvent, TResult>(TEvent request, CancellationToken token)
	{
		this.logger.LogInformation("Executing event [Event = {Name}]", typeof(TEvent).Name);

		var coordinator = this.coordinatorResolver.Resolve<TEvent, TResult>();
		return coordinator.ExecuteAsync(request, token);
	}
}