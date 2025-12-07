using Zenject;

public class WinAppState : AppStateWithUIBase<WinWindow, WinWindowModel>
{
    [Inject] private LevelService _levelService;

    public WinAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService) { }

    protected override void SubscribeUICallBacks()
    {
        _model.OnNext += NextLevel;
    }

    private void NextLevel()
    {
        _levelService.NextLevel();
    }

    protected override void UnSubscribeUICallBacks()
    {
        _model.OnNext -= NextLevel;
    }
}