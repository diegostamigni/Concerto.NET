using Concerto.Coordinator.Attributes;
using Concerto.Coordinator.Contracts;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile.Executors;

[StateMachineTransitionExecutorFor(ImportProfileTrigger.Fail)]
public class ImportProfileFailStateMachineTransitionExecutor
	: IStateMachineTransitionExecutor<ImportProfileState, ImportProfileTrigger>
{
	public Task ExecuteAsync(
		IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler,
		StateMachine<ImportProfileState, ImportProfileTrigger>.Transition transition) => Task.CompletedTask;
}