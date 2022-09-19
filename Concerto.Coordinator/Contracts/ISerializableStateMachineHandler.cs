using System.Text.Json;

namespace Concerto.Coordinator.Contracts;

public interface ISerializableStateMachineHandler
{
	static T? FromJson<T>(string json) where T : ISerializableStateMachineHandler
		=> JsonSerializer.Deserialize<T>(json);

	string ToJson()
		=> JsonSerializer.Serialize(this);
}