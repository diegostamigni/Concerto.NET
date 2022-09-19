namespace Concerto.Coordinator.Contracts;

public interface ICoordinator<in TEvent>
{
	Task ExecuteAsync(TEvent request, CancellationToken token = default);
}

public interface ICoordinator<in TEvent, TResult>
{
	Task<TResult> ExecuteAsync(TEvent request, CancellationToken token = default);
}