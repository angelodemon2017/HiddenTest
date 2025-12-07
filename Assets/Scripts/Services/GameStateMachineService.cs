using System;
using Zenject;

public class GameStateMachineService : IDisposable, ITickable
{
    private SignalBus _signalBus;
    private DiContainer _container;
    private IAppState _currentState;

    public GameStateMachineService(
        SignalBus signalBus,
        DiContainer container)
    {
        _signalBus = signalBus;
        _container = container;
    }

    public void EnterState<T>() where T : IAppState
    {
        _currentState?.Exit();
        _currentState = _container.Resolve<T>();
        _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Run();
    }

    public void Dispose()
    {

    }
}