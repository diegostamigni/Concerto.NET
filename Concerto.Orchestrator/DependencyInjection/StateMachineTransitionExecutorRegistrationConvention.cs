using System.Reflection;
using BaselineTypeDiscovery;
using Concerto.Coordinator.Attributes;
using Concerto.Coordinator.Contracts;
using Lamar;
using Lamar.Scanning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Concerto.Orchestrator.DependencyInjection;

public class StateMachineTransitionExecutorRegistrationConvention : IRegistrationConvention
{
	private readonly ServiceLifetime serviceLifetime;

	private static readonly HashSet<string> SupportedExecutors = new()
	{
		typeof(IStateMachineTransitionExecutor<,>).Name,
		typeof(IStateMachineTransitionExecutor<,,>).Name
	};

	public StateMachineTransitionExecutorRegistrationConvention(ServiceLifetime serviceLifetime)
	{
		this.serviceLifetime = serviceLifetime;
	}

	public void ScanTypes(TypeSet types, ServiceRegistry services)
	{
		foreach (var transitionExecutorType in types
			.AllTypes()
			.Where(x => x.IsClass &&
				!x.IsAbstract && x.GetInterfaces().Any(z => SupportedExecutors.Contains(z.Name)) &&
				x.IsDefined(typeof(StateMachineTransitionExecutorForAttribute)))
			.Select(x => new
			{
				ServiceType = x,
				InterfaceType = x.GetInterfaces().Single(z => SupportedExecutors.Contains(z.Name)),
				Attribute = x.GetCustomAttribute<StateMachineTransitionExecutorForAttribute>()
			}))
		{
			var stateAndTrigger = transitionExecutorType.InterfaceType.GetGenericArguments();
			var nameBuilder = new List<string?>
			{
				transitionExecutorType.InterfaceType.Name,
				stateAndTrigger[0].Name,
				stateAndTrigger[1].Name,
				transitionExecutorType.Attribute?.Trigger.ToString()
			};

			// Has result
			if (stateAndTrigger.Length == 3)
			{
				nameBuilder.Insert(1, stateAndTrigger[2].Name);
			}

			services
				.For(transitionExecutorType.InterfaceType)
				.Use(transitionExecutorType.ServiceType)
				.Named(string.Join("_", nameBuilder))
				.Lifetime = this.serviceLifetime;
		}
	}
}