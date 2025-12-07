using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("MonoServices")]
    [SerializeField] private InputService _inputService;

    [Header("Configs")]
    [SerializeField] private CommonGameConfig _commonGameConfig;
    [SerializeField] private LevelsConfig _levelsConfig;
    [SerializeField] private LevelsSequence _levelsSequence;

    [Header("Prefabs")]
    [SerializeField] private Canvas _canvasPrefab;
    [SerializeField] private FailWindow _failWindow;
    [SerializeField] private LoadingWindow _loadingWindow;
    [SerializeField] private MainMenuWindow _mainMenuWindow;
    [SerializeField] private GameplayWindow _gameplayWindow;
    [SerializeField] private PauseMenuWindow _pauseMenuWindow;
    [SerializeField] private WinWindow _winWindow;

    [SerializeField] private FindingElementViewUI _findingElementViewUIPrefab;

    public override void InstallBindings()
    {
        InstallSignals();
        InstallConfigs();
        InstallBrefabs();
        InstallModels();
        InstallUI();
        InstallServices();
        InitMonoServices();
        InstallAppStates();
    }

    private void InstallConfigs()
    {
        Container.BindInstance(_commonGameConfig).AsSingle();
        Container.BindInstance(_levelsConfig).AsSingle();
        Container.BindInstance(_levelsSequence).AsSingle();
    }

    private void InstallBrefabs()
    {
        Container.BindInstance(_canvasPrefab).WithId(Dicts.DiPrefabIds.Canvas);

        Container.BindMemoryPool<FindingElementViewUI, FindingElementViewUI.Pool>()
            .WithInitialSize(3)
            .FromComponentInNewPrefab(_findingElementViewUIPrefab)
            .AsCached();

        Container.Bind<IFindingElementViewUIFactory>()
            .To<FindingElementViewUIFactory>()
            .AsSingle();
    }

    private void InstallModels()
    {
        Container.Bind<WindowModelBase>().AsSingle();

        Container.Bind<FailWindowModel>().AsSingle();
        Container.Bind<FindingElementsModel>().AsSingle();
        Container.Bind<LoadingWindowModel>().AsSingle();
        Container.Bind<WinWindowModel>().AsSingle();
    }

    private void InstallUI()
    {
        Container.Bind<FailWindow>().FromComponentInNewPrefab(_failWindow).AsSingle();
        Container.Bind<GameplayWindow>().FromComponentInNewPrefab(_gameplayWindow).AsSingle();
        Container.Bind<LoadingWindow>().FromComponentInNewPrefab(_loadingWindow).AsSingle();
        Container.Bind<MainMenuWindow>().FromComponentInNewPrefab(_mainMenuWindow).AsSingle();
        Container.Bind<PauseMenuWindow>().FromComponentInNewPrefab(_pauseMenuWindow).AsSingle();
        Container.Bind<WinWindow>().FromComponentInNewPrefab(_winWindow).AsSingle();
    }

    private void InstallAppStates()
    {
        Container.Bind<FailAppState>().AsSingle();
        Container.Bind<FindingElementsAppState>().AsSingle();
        Container.Bind<LoadingAppState>().AsSingle();
        Container.Bind<MainMenuAppState>().AsSingle();
        Container.Bind<PauseAppState>().AsSingle();
        Container.Bind<WinAppState>().AsSingle();
    }

    private void InstallServices()
    {
        Container.BindInterfacesAndSelfTo<DataService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachineService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimerService>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIService>().AsSingle();
    }

    private void InitMonoServices()
    {
        Container.BindInstance(_inputService).AsSingle();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<AddElementSignal>();
        Container.DeclareSignal<ChangeSceneSignal>();
        Container.DeclareSignal<RemoveElementSignal>();
        Container.DeclareSignal<RunLevelSignal>();
        Container.DeclareSignal<StartNewGameSignal>();
        Container.DeclareSignal<TimerEndSignal>();
    }
}