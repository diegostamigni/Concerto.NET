using Concerto.Coordinator.Contracts;
using Stateless;

namespace Concerto.Coordinator;

public abstract class BaseStateMachineHandler<TState, TTrigger> : IStateMachineHandler<TState, TTrigger>
{
	public TState CurrentState => this.stateMachine.State;

	public WeakReference<IStateMachineHandlerNotifier<TState, TTrigger>>? WeakNotifier { get; set; }

	private readonly StateMachine<TState, TTrigger> stateMachine;

	protected BaseStateMachineHandler(IStateMachineConfiguration<TState, TTrigger> configuration)
	{
		this.stateMachine = new(configuration.InitialState);
		this.stateMachine.OnTransitionedAsync(HandleOnTransitionedAsync);

		foreach (var stateMachineConfig in configuration.Configure(this.stateMachine))
		{
			stateMachineConfig.OnEntryAsync(OnEnterAsync);
			stateMachineConfig.OnExitAsync(OnExitAsync);
		}
	}

	public abstract Task StartAsync();

	public Task FireAsync(TTrigger trigger)
		=> this.stateMachine.FireAsync(trigger);

	public Task FireAsync<TParameter>(TTrigger trigger, TParameter parameter)
	{
		var paramTrigger = this.stateMachine.SetTriggerParameters<TParameter>(trigger);
		return this.stateMachine.FireAsync(paramTrigger, parameter);
	}

	private async Task HandleOnTransitionedAsync(StateMachine<TState, TTrigger>.Transition transition)
	{
		if (this.WeakNotifier is null || !this.WeakNotifier.TryGetTarget(out var target))
		{
			return;
		}

		await target.OnAllTransitionedAsync(this.stateMachine, transition);

		if (transition.Source?.Equals(transition.Destination) == true)
		{
			await target.OnReEntryTransitionedAsync(this.stateMachine, transition);
		}
		else
		{
			await target.OnStatusTransitionedAsync(this.stateMachine, transition);
		}
	}

	private Task OnEnterAsync(StateMachine<TState, TTrigger>.Transition transition)
		=> this.WeakNotifier is not null && this.WeakNotifier.TryGetTarget(out var target)
			? target.OnEnterAsync(this.stateMachine, transition)
			: Task.CompletedTask;

	private  Task OnExitAsync(StateMachine<TState, TTrigger>.Transition transition)
		=> this.WeakNotifier is not null && this.WeakNotifier.TryGetTarget(out var target)
			? target.OnExitAsync(this.stateMachine, transition)
			: Task.CompletedTask;
}