using System.Reflection;
using Concerto.Coordinator;
using Concerto.Coordinator.Contracts;
using Concerto.Mapping.DependencyInjection;
using Concerto.Orchestrator.Settings;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concerto.Orchestrator.DependencyInjection;

public class OrchestratorRegistry : ServiceRegistry
{
	public OrchestratorRegistry(Assembly? coordinatorsAssembly = null)
		: this(ServiceLifetime.Scoped, coordinatorsAssembly)
	{
	}

	public OrchestratorRegistry(ServiceLifetime serviceLifetime, Assembly? coordinatorsAssembly = null)
	{
		Scan(cfg =>
		{
			cfg.AssemblyContainingType(typeof(BaseStateMachineHandler<,>));
			cfg.AssemblyContainingType(typeof(BaseCoordinator<,,,>));

			if (coordinatorsAssembly is not null)
			{
				cfg.Assembly(coordinatorsAssembly);
			}

			cfg.AddAllTypesOf(typeof(IStateMachineHandler<,>), serviceLifetime);
			cfg.AddAllTypesOf(typeof(IStateMachineConfiguration<,>), serviceLifetime);
			cfg.AddAllTypesOf(typeof(ICoordinator<,>), serviceLifetime);

			cfg.With(new StateMachineTransitionExecutorRegistrationConvention(serviceLifetime));
			cfg.WithDefaultConventions(serviceLifetime);
		});

		var configuration = GetConfiguration();

		RegisterConfig<EventSettings>(configuration);

		var serviceBusSettings = RegisterConfig<ServiceBusSettings>(configuration);
		IncludeRegistry(new ServiceBusRegistry(ServiceLifetime.Scoped, serviceBusSettings!));
		IncludeRegistry(new AutoMapperRegistry(ServiceLifetime.Scoped));
	}

	private static IConfiguration GetConfiguration()
	{
		var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
		var configurationBuilder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false, true)
			.AddJsonFile($"appsettings.{environment}.json", true, true)
			.AddEnvironmentVariables();

		var configurationRoot = configurationBuilder.Build();
		return configurationRoot;
	}

	private TConfig? RegisterConfig<TConfig>(IConfiguration configuration) where TConfig : class
	{
		var config = configuration.GetSection(typeof(TConfig).Name).Get<TConfig>();

		if (config is not null)
		{
			this.AddSingleton(config);
		}

		return config;
	}
}