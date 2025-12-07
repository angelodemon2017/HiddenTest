using UnityEngine;
using Zenject;

public class FindingElementController : BaseMonoLevel
{
    [SerializeField] private Transform _pointOfSpawnContainer;

    private SignalBus _signalBus;
    private LevelService _levelService;
    private FindingElementsModel _findingElementsModel;
    private GameStateMachineService _gameStateMachineService;
    private TimerService _timerService;

    [Inject]
    public void Constructor(
        SignalBus signalBus,
        LevelService levelService,
        FindingElementsModel findingElementsModel,
        GameStateMachineService gameStateMachineService,
        TimerService timerService)
    {
        _signalBus = signalBus;
        _levelService = levelService;
        _findingElementsModel = findingElementsModel;
        _gameStateMachineService = gameStateMachineService;
        _timerService = timerService;

        RunConfig(_levelService.TargetConfig);
        _signalBus.Subscribe<TimerEndSignal>(Handle);
    }

    public override void RunConfig(LevelConfigBase levelConfigBase)
    {
        if (levelConfigBase is LevelFindObjectsConfig lfoc)
        {
            if (_findingElementsModel.ElementsContainer != null)
            {
                _findingElementsModel.ElementsContainer.OnElementClicked -= ClickElement;
            }
            _pointOfSpawnContainer.DestroyChildrens();
            _findingElementsModel.ElementsContainer = Instantiate(lfoc.ElementsContainer, _pointOfSpawnContainer);
            _findingElementsModel.ElementsContainer.StartLevelWithConfig(lfoc);
            _findingElementsModel.IdsForSearching.Clear();
            for (var i = 0; i < lfoc.CountElementsToFindTogether; i++)
            {
                if (_findingElementsModel.ElementsContainer.IsLeftItems)
                {
                    _findingElementsModel.IdsForSearching
                        .Add(_findingElementsModel.ElementsContainer.GetNextElementID());
                }
            }
            _findingElementsModel.ElementsContainer.OnElementClicked += ClickElement;
            if (lfoc.Seconds > 0)
            {
                _timerService.StartTimer(lfoc.Seconds);
            }
            else
            {
                _timerService.StopTimer();
            }
            _gameStateMachineService.EnterState<FindingElementsAppState>();
        }
        else
        {
            Debug.LogError("Invalid level config type for FindingElementController");
        }
    }

    private void ClickElement(string id)
    {
        if (_findingElementsModel.IdsForSearching.Contains(id))
        {
            _findingElementsModel.IdsForSearching.Remove(id);
            _findingElementsModel.ElementsContainer.HideElement(id);
            _signalBus.Fire(new RemoveElementSignal(id));
            if (_findingElementsModel.ElementsContainer.IsLeftItems)
            {
                var nextElement = _findingElementsModel.ElementsContainer.GetNextElementID();
                _findingElementsModel.IdsForSearching.Add(nextElement);
                _signalBus.Fire(new AddElementSignal(nextElement));
            }
            else
            {
                if (_findingElementsModel.IdsForSearching.Count == 0)
                {
                    FinishLevel();
                }
            }
        }
    }

    private void Handle(TimerEndSignal timerEnd)
    {
        FailLevel();
    }

    private void FailLevel()
    {
        _gameStateMachineService.EnterState<FailAppState>();
    }

    private void FinishLevel()
    {
        _timerService.StopTimer();
        _findingElementsModel.ElementsContainer.EndLevel();
        _gameStateMachineService.EnterState<WinAppState>();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<TimerEndSignal>(Handle);
    }
}