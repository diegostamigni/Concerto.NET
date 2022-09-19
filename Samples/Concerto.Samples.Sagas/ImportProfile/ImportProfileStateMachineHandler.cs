using Concerto.Coordinator;
using Concerto.Coordinator.Contracts;

namespace Concerto.Samples.Sagas.ImportProfile;

public class ImportProfileStateMachineHandler : BaseStateMachineHandler<ImportProfileState, ImportProfileTrigger>
{
	public ImportProfileStateMachineHandler(
		IStateMachineConfiguration<ImportProfileState, ImportProfileTrigger> configuration)
		: base(configuration)
	{
	}

	public override Task StartAsync() => FireAsync(ImportProfileTrigger.Create);
}