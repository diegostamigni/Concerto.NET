using Concerto.Coordinator;
using Concerto.Coordinator.Contracts;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile;

public class ImportProfileCoordinator
	: BaseCoordinator<ImportProfileSagaEvent, ImportProfileResponse?, ImportProfileState, ImportProfileTrigger>
{
	private readonly TaskCompletionSource<ImportProfileResponse> taskCompletionSource = new();

	private readonly IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler;
	private readonly IStateMachineTransitionExecutorResolver transitionExecutorResolver;

	public ImportProfileCoordinator(
		IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler,
		IStateMachineTransitionExecutorResolver transitionExecutorResolver)
	{
		this.stateMachineHandler = stateMachineHandler;
		this.transitionExecutorResolver = transitionExecutorResolver;

		this.stateMachineHandler.WeakNotifier = new(this);
	}

	public override async Task<ImportProfileResponse?> ExecuteAsync(
		ImportProfileSagaEvent request,
		CancellationToken token = default)
	{
		await this.stateMachineHandler.FireAsync(ImportProfileTrigger.Create, request);
		return await this.taskCompletionSource.Task;
	}

	public override async Task OnAllTransitionedAsync(
		StateMachine<ImportProfileState, ImportProfileTrigger> stateMachine,
		StateMachine<ImportProfileState, ImportProfileTrigger>.Transition transition)
	{
		switch (transition.Trigger)
		{
			case ImportProfileTrigger.Complete:
				var completeExecutor = this.transitionExecutorResolver
					.Resolve<ImportProfileState, ImportProfileTrigger, ImportProfileResponse>(transition.Trigger);
				var result = await completeExecutor.ExecuteAsync(this.stateMachineHandler, transition);
				this.taskCompletionSource.SetResult(result);
				break;

			default:
				var transitionExecutor = this.transitionExecutorResolver
					.Resolve<ImportProfileState, ImportProfileTrigger>(transition.Trigger);
				await transitionExecutor.ExecuteAsync(this.stateMachineHandler, transition);
				break;
		}
	}
}