namespace Concerto.Coordinator.Contracts;

public interface ICoordinatorResolver
{
	ICoordinator<TEvent> Resolve<TEvent>();

	ICoordinator<TEvent, TResult> Resolve<TEvent, TResult>();
}