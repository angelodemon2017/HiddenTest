using Zenject;

public class MainMenuAppState : AppStateWithUIBase<MainMenuWindow, WindowModelBase>
{
    [Inject] private GameStateMachineService _gameStateMachineService;

    public MainMenuAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService)
    {

    }

    protected override void SubscribeUICallBacks()
    {
        _uIWindow.StartNewGameAction += OnStartNewGame;
    }

    private void OnStartNewGame()
    {
        _signalBus.Fire<StartNewGameSignal>();
    }

    protected override void UnSubscribeUICallBacks()
    {
        _uIWindow.StartNewGameAction -= OnStartNewGame;
    }
}