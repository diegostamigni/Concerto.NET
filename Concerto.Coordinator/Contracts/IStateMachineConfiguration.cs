using Stateless;

namespace Concerto.Coordinator.Contracts;

public interface IStateMachineConfiguration<TState, TTrigger>
{
	TState InitialState { get; }
	IEnumerable<StateMachine<TState, TTrigger>.StateConfiguration> Configure(
		StateMachine<TState, TTrigger> stateMachine);
}