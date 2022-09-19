using Concerto.Orchestrator.Settings;
using EasyNetQ;
using EasyNetQ.ConnectionString;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace Concerto.Orchestrator.DependencyInjection;

public class ServiceBusRegistry : ServiceRegistry
{
	public ServiceBusRegistry(ServiceBusSettings settings)
		: this(ServiceLifetime.Scoped, settings)
	{
	}

	public ServiceBusRegistry(ServiceLifetime serviceLifetime, ServiceBusSettings settings)
	{
		var serviceRegister = new LamarAdapter(this, serviceLifetime);
		RabbitHutch.RegisterBus(
			serviceRegister,
			resolver => resolver.Resolve<IConnectionStringParser>().Parse(settings.ConnectionString),
			_ => {});
	}
}