namespace Concerto.Samples.Sagas.ImportProfile;

public enum ImportProfileTrigger
{
	Pending,
	Fail,
	Create,
	Complete,
	Notify,
	AssociateTag,
	UploadAvatar,
	UploadSearchIndex,
	CreateMessagingUser,
	CreateServiceDeskTicket,
	CreateCustomer
}