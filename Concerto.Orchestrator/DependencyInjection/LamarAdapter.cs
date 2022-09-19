using EasyNetQ.DI;
using Lamar;
using Lamar.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace Concerto.Orchestrator.DependencyInjection;

/// <inheritdoc />
internal class LamarAdapter : IServiceRegister
{
	private readonly ServiceRegistry registry;

	/// <summary>
	/// Creates an adapter on top of IRegistry
	/// </summary>
	public LamarAdapter(ServiceRegistry registry, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
	{
		this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
		this.registry.For<IServiceResolver>().Use<LamarResolver>().Lifetime = serviceLifetime;
	}

	/// <inheritdoc />
	public IServiceRegister Register(
		Type serviceType,
		Func<IServiceResolver, object> implementationFactory,
		Lifetime lifetime = Lifetime.Singleton)
	{
		var lamarLifetime = lifetime switch
		{
			Lifetime.Transient => ServiceLifetime.Transient,
			Lifetime.Singleton => ServiceLifetime.Singleton,
			_ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
		};

		this.registry
			.For(serviceType)
			.Use(new LambdaInstance(
				serviceType,
				provider => implementationFactory(provider.GetRequiredService<IServiceResolver>()),
				lamarLifetime));

		return this;
	}

	/// <inheritdoc />
	public IServiceRegister TryRegister(
		Type serviceType,
		Func<IServiceResolver, object> implementationFactory,
		Lifetime lifetime = Lifetime.Singleton)
	{
		var lamarLifetime = lifetime switch
		{
			Lifetime.Transient => ServiceLifetime.Transient,
			Lifetime.Singleton => ServiceLifetime.Singleton,
			_ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
		};

		this.registry
			.For(serviceType)
			.Use(new LambdaInstance(
				serviceType,
				provider => implementationFactory(provider.GetRequiredService<IServiceResolver>()),
				lamarLifetime));

		return this;
	}

	/// <inheritdoc />
	public IServiceRegister Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Singleton)
	{
		switch (lifetime)
		{
			case Lifetime.Transient:
				this.registry.For(serviceType).Use(implementingType).Lifetime = ServiceLifetime.Transient;
				return this;
			case Lifetime.Singleton:
				this.registry.For(serviceType).Use(implementingType).Lifetime = ServiceLifetime.Singleton;
				return this;
			default:
				throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
		}
	}

	/// <inheritdoc />
	public IServiceRegister TryRegister(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.Singleton)
	{
		switch (lifetime)
		{
			case Lifetime.Transient:
				this.registry.For(serviceType).Use(implementationType).Lifetime = ServiceLifetime.Transient;
				return this;
			case Lifetime.Singleton:
				this.registry.For(serviceType).Use(implementationType).Lifetime = ServiceLifetime.Singleton;
				return this;
			default:
				throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
		}
	}

	/// <inheritdoc />
	public IServiceRegister Register(Type serviceType, object implementationInstance)
	{
		this.registry
			.For(serviceType)
			.Use(new ObjectInstance(serviceType, implementationInstance));

		return this;
	}

	/// <inheritdoc />
	public IServiceRegister TryRegister(Type serviceType, object implementationInstance)
	{
		this.registry
			.For(serviceType)
			.Use(new ObjectInstance(serviceType, implementationInstance));

		return this;
	}

	private class LamarResolver : IServiceResolver
	{
		protected readonly IContainer Container;

		public LamarResolver(IContainer container)
			=> this.Container = container;

		public TService Resolve<TService>() where TService : class
			=> this.Container.GetInstance<TService>();

		public IServiceResolverScope CreateScope()
			=> new LamarResolverScope(this.Container);
	}

	private class LamarResolverScope : LamarResolver, IServiceResolverScope
	{
		public LamarResolverScope(IContainer container)
			: base(container)
		{
		}

		public void Dispose()
			=> this.Container.Dispose();
	}
}