using Concerto.Coordinator.Contracts;
using Lamar;

namespace Concerto.Coordinator;

public class CoordinatorResolver : ICoordinatorResolver
{
	private readonly IContainer container;

	public CoordinatorResolver(IContainer container)
		=> this.container = container;

	public ICoordinator<TEvent> Resolve<TEvent>() => this.container.GetInstance<ICoordinator<TEvent>>();

	public ICoordinator<TEvent, TResult> Resolve<TEvent, TResult>()
		=> this.container.GetInstance<ICoordinator<TEvent, TResult>>();
}