using UnityEngine;
using Zenject;

public abstract class AppStateBase : IAppState
{
    [Inject] protected DiContainer _diContainer;
    protected SignalBus _signalBus;
    protected InputService _inputService;
    protected virtual bool _cursorIsAvaiable => true;

    public AppStateBase(
        SignalBus signalBus,
        InputService inputService)
    {
        _signalBus = signalBus;
        _inputService = inputService;
    }

    public void Enter()
    {
        EnterAppState();
        InputSubscribe();
        CursorIs(!_cursorIsAvaiable);
    }

    protected virtual void EnterAppState()
    {

    }

    protected virtual void InputSubscribe()
    {

    }

    public virtual void Run()
    {

    }

    public void Exit()
    {
        ExitAppState();
        InputUnsubscribe();
    }

    protected virtual void ExitAppState()
    {

    }

    protected virtual void InputUnsubscribe()
    {

    }

    private void CursorIs(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLock;
    }
}