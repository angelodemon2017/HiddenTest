using Zenject;

public class FindingElementsAppState : AppStateWithUIBase<GameplayWindow, FindingElementsModel>
{
    [Inject] private GameStateMachineService _gameStateMachineService;

    public FindingElementsAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService)
    {

    }

    protected override void InputSubscribe()
    {
        _inputService.EscapeAction += OnEscapePressed;
    }

    private void OnEscapePressed()
    {
        _gameStateMachineService.EnterState<PauseAppState>();
    }

    protected override void InputUnsubscribe()
    {
        _inputService.EscapeAction -= OnEscapePressed;
    }
}