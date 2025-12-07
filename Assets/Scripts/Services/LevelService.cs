using System;
using Zenject;

public class LevelService : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly LevelsSequence _levelsSequence;
    private readonly DataService _dataService;

    private LevelConfigBase _targetConfig;

    public LevelConfigBase TargetConfig => _targetConfig;

    public LevelService(
        SignalBus signalBus,
        LevelsSequence levelsSequence,
        DataService dataService)
    {
        _signalBus = signalBus;
        _levelsSequence = levelsSequence;
        _dataService = dataService;

        _signalBus.Subscribe<StartNewGameSignal>(Handle);
        _signalBus.Subscribe<RunLevelSignal>(Handle);
    }

    private void Handle(StartNewGameSignal startNewGame)
    {
        _dataService.StartNewGame();
        _signalBus.Fire(
            new RunLevelSignal(
                _levelsSequence.GetConfigByIndex(_dataService.CurrentLevel)));
    }

    private void Handle(RunLevelSignal runLevel)
    {
        _targetConfig = runLevel.LevelConfig;
        var scene = _levelsSequence.GetSceneIdByIndex(_dataService.CurrentLevel);

        _signalBus.Fire(new ChangeSceneSignal(scene));
    }

    public void NextLevel()
    {
        _dataService.NextLevel();
        _signalBus.Fire(
            new RunLevelSignal(
                _levelsSequence.GetConfigByIndex(_dataService.CurrentLevel)));
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<StartNewGameSignal>(Handle);
        _signalBus.Unsubscribe<RunLevelSignal>(Handle);
    }
}