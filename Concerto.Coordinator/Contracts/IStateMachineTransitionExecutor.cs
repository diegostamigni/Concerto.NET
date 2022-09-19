using Stateless;

namespace Concerto.Coordinator.Contracts;

public interface IStateMachineTransitionExecutor<TState, TTrigger>
{
	Task ExecuteAsync(
		IStateMachineHandler<TState, TTrigger> stateMachineHandler,
		StateMachine<TState, TTrigger>.Transition transition);
}

public interface IStateMachineTransitionExecutor<TState, TTrigger, TResult>
{
	Task<TResult> ExecuteAsync(
		IStateMachineHandler<TState, TTrigger> stateMachineHandler,
		StateMachine<TState, TTrigger>.Transition transition);
}