namespace Concerto.Orchestrator.Settings;

public class EventWithResult
{
	public string EventTypeName { get; set; } = null!;

	public string EventTypeResult { get; set; } = null!;

	public string? EventAssemblyName { get; init; }
}