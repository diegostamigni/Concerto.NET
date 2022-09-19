namespace Concerto.Samples.Sagas.ImportProfile;

public enum ImportProfileState
{
	Pending,
	Failed,
	Completed,
	Created,
	Notified,
	TagAssociated,
	AvatarUploaded,
	SearchIndexUploaded,
	MessagingUserCreated,
	ServiceDeskTicketCreated,
	CustomerCreated
}