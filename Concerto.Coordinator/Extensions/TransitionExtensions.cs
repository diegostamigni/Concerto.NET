using Stateless;

namespace Concerto.Coordinator.Extensions;

public static class TransitionExtensions
{
	public static TParameter? Get<TState, TTrigger, TParameter>(
		this StateMachine<TState, TTrigger>.Transition transition) => transition.Parameters
		.Where(x => x is TParameter)
		.Cast<TParameter>()
		.FirstOrDefault();
	public static TParameter?[] GetAllAs<TState, TTrigger, TParameter>(
		this StateMachine<TState, TTrigger>.Transition transition) => transition.Parameters
		.Where(x => x is TParameter)
		.Cast<TParameter>()
		.ToArray();
}