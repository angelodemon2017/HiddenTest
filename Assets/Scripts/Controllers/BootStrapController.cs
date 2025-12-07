using Zenject;

public class BootStrapController : BaseMonoLevel
{
    [Inject] private GameStateMachineService _gameStateMachineService;

    private void Start()
    {
        _gameStateMachineService.EnterState<MainMenuAppState>();
    }
}