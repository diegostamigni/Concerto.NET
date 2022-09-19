namespace Concerto.Coordinator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class StateMachineTransitionExecutorForAttribute : Attribute
{
	public object Trigger { get; }

	public StateMachineTransitionExecutorForAttribute(object trigger)
	{
		this.Trigger = trigger;
	}
}