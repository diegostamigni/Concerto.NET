using Concerto.Orchestrator;
using Concerto.Orchestrator.DependencyInjection;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
	.CreateDefaultBuilder(args)
	.UseLamar()
	.ConfigureServices(services =>
	{
		services.AddLogging();
		services.AddHostedService<Worker>();
	})
	.ConfigureContainer<Lamar.ServiceRegistry>((_, services) =>
	{
		services.IncludeRegistry(new OrchestratorRegistry(ServiceLifetime.Transient));
	})
	.Build();

await host.RunAsync();