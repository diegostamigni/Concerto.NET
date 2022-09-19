using AutoMapper;
using Concerto.Coordinator.Attributes;
using Concerto.Coordinator.Contracts;
using Concerto.Coordinator.Extensions;
using EasyNetQ;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile.Executors;

[StateMachineTransitionExecutorFor(ImportProfileTrigger.Complete)]
public class ImportProfileCompleteStateMachineTransitionExecutor
	: IStateMachineTransitionExecutor<ImportProfileState, ImportProfileTrigger, ImportProfileResponse?>
{
	private readonly IBus bus;
	private readonly IMapper mapper;

	public ImportProfileCompleteStateMachineTransitionExecutor(IBus bus, IMapper mapper)
	{
		this.bus = bus;
		this.mapper = mapper;
	}

	public Task<ImportProfileResponse?> ExecuteAsync(
		IStateMachineHandler<ImportProfileState, ImportProfileTrigger> stateMachineHandler,
		StateMachine<ImportProfileState, ImportProfileTrigger>.Transition transition)
	{
		var result = transition.Get<ImportProfileState, ImportProfileTrigger, ImportProfileResponse>();

		// Trigger event
		// await this.bus.PubSub.PublishAsync<ImportProfileCompletedEvent>(response);

		return Task.FromResult(result);
	}
}