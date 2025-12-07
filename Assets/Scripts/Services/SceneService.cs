using System;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneService : IDisposable
{
    private readonly LoadingWindowModel _loadingWindowModel;
    private readonly SignalBus _signalBus;
    private readonly GameStateMachineService _gameStateMachineService;

    private BaseMonoLevel _baseMonoLevel;

    private string _targetScene;

    public BaseMonoLevel CurrentLevel => _baseMonoLevel;
    public string CurrentScene => SceneManager.GetActiveScene().name;

    public SceneService(
        LoadingWindowModel loadingWindowModel,
        SignalBus signalBus,
        GameStateMachineService gameStateMachineService)
    {
        _signalBus = signalBus;
        _loadingWindowModel = loadingWindowModel;
        _gameStateMachineService = gameStateMachineService;

        _loadingWindowModel.Showed += LoadingWindowShowed;
        _signalBus.Subscribe<ChangeSceneSignal>(LoadLevel);
    }

    public void SetLevel(BaseMonoLevel baseMonoLevel)
    {
        _baseMonoLevel = baseMonoLevel;
    }

    private void LoadLevel(ChangeSceneSignal sceneSignal)
    {
        LoadLevel(sceneSignal.SceneName);
    }

    private void LoadLevel(string sceneName)
    {
        _targetScene = sceneName;

        _gameStateMachineService.EnterState<LoadingAppState>();
    }

    private void LoadingWindowShowed()
    {
        SceneManager.LoadScene(_targetScene);
    }

    public void Dispose()
    {
        _loadingWindowModel.Showed -= LoadingWindowShowed;
        _signalBus.Unsubscribe<ChangeSceneSignal>(LoadLevel);
    }
}