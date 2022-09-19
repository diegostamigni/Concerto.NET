using System.Reflection;
using Concerto.Coordinator.Contracts;
using Concerto.Mapping.DependencyInjection;
using Concerto.Orchestrator.DependencyInjection;
using Concerto.Samples.Sagas.ImportProfile;
using EasyNetQ;
using Lamar;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;

namespace Concerto.Orchestrator.Tests;

public abstract class BaseCoordinatorTests
{
	protected IContainer Container { get; private set; }

	protected Mock<IBus> BusMock { get; } = new();
	protected Mock<IPubSub> PubSubMock { get; } = new();
	protected Mock<IRpc> RpcMock { get; } = new();

	[SetUp]
	public void BaseSetUp()
	{
		this.Container = new Container(cfg =>
		{
			cfg.IncludeRegistry(new OrchestratorRegistry(Assembly.GetAssembly(typeof(ImportProfileCoordinator))));
			cfg.IncludeRegistry(new AutoMapperRegistry());

			SetupServiceBus(cfg);
		});

		this.BusMock.Setup(x => x.PubSub).Returns(this.PubSubMock.Object);
		this.BusMock.Setup(x => x.Rpc).Returns(this.RpcMock.Object);
	}

	[TearDown]
	public void TearDown()
	{
		this.RpcMock.Reset();
		this.PubSubMock.Reset();
		this.BusMock.Reset();
	}

	protected virtual void SetupServiceBus(ServiceRegistry serviceRegistry)
	{
		serviceRegistry.RemoveAll<IBus>();
		serviceRegistry.For<IBus>().Use(this.BusMock.Object);
	}
}

public abstract class BaseCoordinatorTests<TEvent> : BaseCoordinatorTests
{
	protected ICoordinator<TEvent> Coordinator { get; private set; } = null!;

	[SetUp]
	public void BaseOtherSetUp()
	{
		this.Coordinator = this.Container.GetInstance<ICoordinator<TEvent>>();
	}
}

public abstract class BaseCoordinatorTests<TEvent, TResult> : BaseCoordinatorTests
{
	protected ICoordinator<TEvent, TResult> Coordinator { get; private set; } = null!;

	[SetUp]
	public void BaseOtherSetUp()
	{
		this.Coordinator = this.Container.GetInstance<ICoordinator<TEvent, TResult>>();
	}
}