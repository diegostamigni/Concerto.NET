Concerto
==========

Concerto is a saga orchestrator architecture I designed for dotnet. This is an ideal implementation that I typically when I need to implement sagas.

## Components

### Coordinator
Events should be of type `ICoordinator<in TEvent>` or `ICoordinator<in TEvent, TResult>`. A base class is provided for convenience: `BaseCoordinator<TEvent, TResult, TState, TTrigger>`.
As per Orchestrator definition, an event should be triggered by a Trigger. Hence, a pair of enums should be provided to define the combination of Trigger and State. A state machine
configuration should then be setup to define the flow and changes on state change. A few contracts are provided to guarantee this is done correctly:
 * `IStateMachineConfiguration<TState, TTrigger>`: defines the state machine configuration
 * `BaseStateMachineHandler<TState, TTrigger>`: defines the state machine handler used by the coordinator
For each pair of state and trigger, we need to define their executors. Executors are defined via the contract `IStateMachineTransitionExecutor<TState, TTrigger>` and there should always
be one per trigger as this is the actual code run by the coordinator on state change. The coordinator will then walk through the state machine and execute the appropriate executor for
each transition.

### Orchestrator
The main component is the Orchestrator service. It runs as dotnet worker service and listens to events triggering sagas. Events (with results) are registered within
the `appsettings.json` config file. Here, this structure allows you to specify incoming events and their assembly:
```json
{
  "SupportedEventsWithResult": [
    {
      "EventTypeName": "Concerto.Samples.Sagas.ImportProfile.ImportProfileSagaEvent",
      "EventTypeResult": "Concerto.Samples.Sagas.ImportProfile.ImportProfileResponse",
      "EventAssemblyName": "Concerto.Samples.Sagas.dll"
    }
  ]
}
```
The assembly is loaded upon service initial bootstrap and the events are mapped and registered for listening. This allows for complete decoupling of the orchestrator and support for
plugin architecture.

The orchestrator uses EasyNetQ internally in order to listen to and dispatch events. This is for simplicity and can be replaced with any other messaging system (eg. Kafka). Also, the
registration for triggering sags is performed via RPC. This is also for simplicity and can be replaced with any other mechanism (eg. REST).

### Samples and tests
The folder `Samples` within this repo contains a simplistic showcase of a typical saga flow. The test suite triggers the saga and verifies the result.

### Disclaimer
This is not supposed to be used as is, but is instead a showcase of how I'd implement a saga orchestrator. It is not production ready and is not intended to be used as such. Take it as a
design exercise on which you'd implement a dynamic orchestrator API.

I've been personally been using this architecture for a few years now and it has proven to be very robust and flexible. It is also very easy to extend, add new features to and configure
for different scenarios. I've used it in a few projects and it has always been a success.