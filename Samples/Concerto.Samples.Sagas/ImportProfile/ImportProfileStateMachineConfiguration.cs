using Concerto.Coordinator.Contracts;
using Stateless;

namespace Concerto.Samples.Sagas.ImportProfile;

public class ImportProfileStateMachineConfiguration : IStateMachineConfiguration<ImportProfileState, ImportProfileTrigger>
{
	public ImportProfileState InitialState => ImportProfileState.Pending;

	public IEnumerable<StateMachine<ImportProfileState, ImportProfileTrigger>.StateConfiguration> Configure(
		StateMachine<ImportProfileState, ImportProfileTrigger> stateMachine)
	{
		yield return stateMachine
			.Configure(ImportProfileState.Pending)
			.Permit(ImportProfileTrigger.Create, ImportProfileState.Created)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.Created)
			.Permit(ImportProfileTrigger.AssociateTag, ImportProfileState.TagAssociated)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.TagAssociated)
			.Permit(ImportProfileTrigger.UploadAvatar, ImportProfileState.AvatarUploaded)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.AvatarUploaded)
			.Permit(ImportProfileTrigger.UploadSearchIndex, ImportProfileState.SearchIndexUploaded)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.SearchIndexUploaded)
			.Permit(ImportProfileTrigger.CreateMessagingUser, ImportProfileState.MessagingUserCreated)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.MessagingUserCreated)
			.Permit(ImportProfileTrigger.CreateServiceDeskTicket, ImportProfileState.ServiceDeskTicketCreated)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.ServiceDeskTicketCreated)
			.Permit(ImportProfileTrigger.CreateCustomer, ImportProfileState.CustomerCreated)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.CustomerCreated)
			.Permit(ImportProfileTrigger.Notify, ImportProfileState.Notified)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.Notified)
			.Permit(ImportProfileTrigger.Complete, ImportProfileState.Completed)
			.Permit(ImportProfileTrigger.Fail, ImportProfileState.Failed);

		yield return stateMachine
			.Configure(ImportProfileState.Completed);

		yield return stateMachine
			.Configure(ImportProfileState.Failed);
	}
}