using Stateless;

namespace Concerto.Coordinator.Contracts;

public interface IStateMachineHandlerNotifier<TState, TTrigger>
{
	Task OnAllTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);

	Task OnReEntryTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);

	Task OnStatusTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);

	Task OnEnterAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);

	Task OnExitAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);
}