using Concerto.Coordinator.Contracts;
using Stateless;

namespace Concerto.Coordinator;

public abstract class BaseCoordinator<TEvent, TResult, TState, TTrigger>
	: ICoordinator<TEvent, TResult>, IStateMachineHandlerNotifier<TState, TTrigger>
{
	public abstract Task<TResult> ExecuteAsync(TEvent request, CancellationToken token = default);

	public abstract Task OnAllTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition);

	public Task OnReEntryTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition) => Task.CompletedTask;

	public Task OnStatusTransitionedAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition) => Task.CompletedTask;

	public Task OnEnterAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition) => Task.CompletedTask;

	public Task OnExitAsync(
		StateMachine<TState, TTrigger> stateMachine,
		StateMachine<TState, TTrigger>.Transition transition) => Task.CompletedTask;
}