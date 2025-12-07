using Zenject;

public class PauseAppState : AppStateWithUIBase<PauseMenuWindow, WindowModelBase>
{
    [Inject] private GameStateMachineService _gameStateMachineService;

    public PauseAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService)
    {

    }

    protected override void InputSubscribe()
    {
        _inputService.EscapeAction += OnEscapePressed;
    }

    private void OnEscapePressed()
    {
        _gameStateMachineService.EnterState<FindingElementsAppState>();
    }

    protected override void InputUnsubscribe()
    {
        _inputService.EscapeAction -= OnEscapePressed;
    }
}