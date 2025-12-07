using Zenject;

public abstract class AppStateWithUIBase<T, Tmodel> : AppStateBase
    where T : UIWindowBase<Tmodel>
    where Tmodel : WindowModelBase
{
    [Inject] private UIService _uiService;

    protected T _uIWindow;
    [Inject] protected Tmodel _model;

    public AppStateWithUIBase(
        SignalBus signalBus,
        InputService inputService)
        : base(signalBus, inputService)
    {

    }

    protected override void EnterAppState()
    {
        _uIWindow = _uiService.ChangeWindow<T>();
        SubscribeUICallBacks();
    }

    protected virtual void SubscribeUICallBacks()
    {

    }

    protected override void ExitAppState()
    {
        UnSubscribeUICallBacks();
    }

    protected virtual void UnSubscribeUICallBacks()
    {

    }
}