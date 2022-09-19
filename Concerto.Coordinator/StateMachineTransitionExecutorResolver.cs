using Concerto.Coordinator.Contracts;
using Lamar;

namespace Concerto.Coordinator;

public class StateMachineTransitionExecutorResolver : IStateMachineTransitionExecutorResolver
{
	private readonly IContainer container;

	public StateMachineTransitionExecutorResolver(IContainer container)
		=> this.container = container;

	public IStateMachineTransitionExecutor<TState, TTrigger> Resolve<TState, TTrigger>(TTrigger trigger)
		=> this.container.GetInstance<IStateMachineTransitionExecutor<TState, TTrigger>>(
			$"{typeof(IStateMachineTransitionExecutor<,>).Name}_{typeof(TState).Name}_{typeof(TTrigger).Name}_{trigger}");

	public IStateMachineTransitionExecutor<TState, TTrigger, TResult> Resolve<TState, TTrigger, TResult>(TTrigger trigger)
		=> this.container.GetInstance<IStateMachineTransitionExecutor<TState, TTrigger, TResult>>(
			$"{typeof(IStateMachineTransitionExecutor<,,>).Name}_{typeof(TResult).Name}_{typeof(TState).Name}_{typeof(TTrigger).Name}_{trigger}");
}