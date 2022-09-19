using System.Reflection;
using AutoMapper;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace Concerto.Mapping.DependencyInjection;

public class AutoMapperRegistry : ServiceRegistry
{
	public AutoMapperRegistry()
		: this(ServiceLifetime.Singleton)
	{
	}

	public AutoMapperRegistry(ServiceLifetime serviceLifetime)
	{
		var mapperConfiguration = new MapperConfiguration(cfg =>
		{
			var addProfileMethodInfo = cfg
				.GetType()
				.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.SingleOrDefault(x =>
					x.IsGenericMethod && x.Name == nameof(IMapperConfigurationExpression.AddProfile));

			if (addProfileMethodInfo is null)
			{
				return;
			}

			foreach (var profile in Assembly
				.GetAssembly(typeof(AutoMapperRegistry))?
				.GetTypes()
				.Where(x => typeof(Profile).IsAssignableFrom(x)) ?? Enumerable.Empty<Type>())
			{
				var createProfileMethod = addProfileMethodInfo.MakeGenericMethod(profile);

				createProfileMethod.Invoke(cfg, null);
			}
		});

		For<IMapper>()
			.Use<Mapper>()
			.Ctor<IConfigurationProvider>().Is(mapperConfiguration)
			.Lifetime = serviceLifetime;
	}
}
