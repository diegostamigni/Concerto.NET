using Concerto.Coordinator.Attributes;
using Concerto.Coordinator.Contracts;
using Concerto.Coordinator.Extensions;
using EasyNetQ;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile.Executors;

[StateMachineTransitionExecutorFor(ImportProfileTrigger.UploadSearchIndex)]
public class ImportProfileUploadSearchIndexStateMachineTransitionExecutor
	: IStateMachineTransitionExecutor<ImportProfileState, ImportProfileTrigger>
{
	private readonly IBus bus;

	public ImportProfileUploadSearchIndexStateMachineTransitionExecutor(IBus bus)
	{
		this.bus = bus;
	}

	public async Task ExecuteAsync(
		IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler,
		StateMachine<ImportProfileState, ImportProfileTrigger>.Transition transition)
	{
		var response = transition.Get<ImportProfileState, ImportProfileTrigger, ImportProfileResponse>();
		if (response?.Id is null)
		{
			await stateMachineHandler.FireAsync(ImportProfileTrigger.Fail);
			return;
		}

		// Fire event
		// await this.bus.PubSub.PublishAsync<UploadSearchIndex>(response);

		await stateMachineHandler.FireAsync(ImportProfileTrigger.CreateMessagingUser, response);
	}
}