using AutoMapper;
using Concerto.Coordinator.Attributes;
using Concerto.Coordinator.Contracts;
using Concerto.Coordinator.Extensions;
using EasyNetQ;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile.Executors;

[StateMachineTransitionExecutorFor(ImportProfileTrigger.Create)]
public class ImportProfileCreateStateMachineTransitionExecutor
	: IStateMachineTransitionExecutor<ImportProfileState, ImportProfileTrigger>
{
	private readonly IBus bus;
	private readonly IMapper mapper;

	public ImportProfileCreateStateMachineTransitionExecutor(IBus bus, IMapper mapper)
	{
		this.bus = bus;
		this.mapper = mapper;
	}

	public async Task ExecuteAsync(
		IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler,
		StateMachine<ImportProfileState, ImportProfileTrigger>.Transition transition)
	{
		var request = transition.Get<ImportProfileState, ImportProfileTrigger, ImportProfileSagaEvent>();
		if (request is null)
		{
			await stateMachineHandler.FireAsync(ImportProfileTrigger.Fail);
			return;
		}

		// Fire event
		// await this.bus.PubSub.PublishAsync<ImportProfileEntityEvent>(response);

		await stateMachineHandler.FireAsync(ImportProfileTrigger.AssociateTag, new ImportProfileResponse
		{
			Id = Guid.NewGuid()
		});
	}
}