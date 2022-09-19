namespace Concerto.Coordinator.Contracts;

public interface IStateMachineHandler<TState, TTrigger> : ISerializableStateMachineHandler
{
	TState CurrentState { get; }

	WeakReference<IStateMachineHandlerNotifier<TState, TTrigger>>? WeakNotifier { get; set; }

	Task FireAsync(TTrigger trigger);
	Task FireAsync<TParameter>(TTrigger trigger, TParameter parameter);
}