namespace Concerto.Coordinator.Contracts;

public interface IStateMachineTransitionExecutorResolver
{
	IStateMachineTransitionExecutor<TState, TTrigger> Resolve<TState, TTrigger>(TTrigger trigger);
	IStateMachineTransitionExecutor<TState, TTrigger, TResult> Resolve<TState, TTrigger, TResult>(TTrigger trigger);
}