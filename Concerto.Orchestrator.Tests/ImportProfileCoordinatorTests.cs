using System.Threading.Tasks;
using Concerto.Samples.Sagas.ImportProfile;
using NUnit.Framework;
using Shouldly;

namespace Concerto.Orchestrator.Tests;

[TestFixture]
public class ImportProfileCoordinatorTests : BaseCoordinatorTests<ImportProfileSagaEvent, ImportProfileResponse>
{
	[Test]
	public async Task ImportProfile_Success()
	{
		var importProfileEvent = new ImportProfileSagaEvent
		{
		};

		var result = await this.Coordinator.ExecuteAsync(importProfileEvent);
		result.ShouldNotBeNull();
		result.ShouldSatisfyAllConditions
		(
			() => result.Id.ShouldNotBeNull()
		);
	}
}