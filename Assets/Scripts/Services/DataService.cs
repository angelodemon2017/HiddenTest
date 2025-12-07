using System;
using Zenject;

public class DataService : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly CommonGameConfig _commonGameConfig;
    private readonly LevelsSequence _levelsSequence;

    private PlayerData _playerData;

    public int CurrentLevel => _playerData.CurrentLevel;
    public EElementsViewMode CurrentElementsViewMode => _playerData.SelectedMode;

    public Action SettingsUpdate;

    public DataService(
        SignalBus signalBus,
        CommonGameConfig commonGameConfig,
        LevelsConfig levelsConfig,
        LevelsSequence levelsSequence)
    {
        _signalBus = signalBus;
        _commonGameConfig = commonGameConfig;
        _levelsSequence = levelsSequence;

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        _playerData = new PlayerData();
        _playerData.SelectedMode = _commonGameConfig.ElementsViewMode;
        _playerData.CurrentLevel = 0;
    }

    public void StartNewGame()
    {
        _playerData.CurrentLevel = 0;
    }

    public void NextLevel()
    {
        _playerData.CurrentLevel++;
        if (_playerData.CurrentLevel >= _levelsSequence.CountOfLevels)
        {
            _playerData.CurrentLevel = _levelsSequence.CountOfLevels - 1;
        }
    }

    public void ChangeViewMode()
    {
        switch (_playerData.SelectedMode)
        {
            case EElementsViewMode.Text:
                _playerData.SelectedMode = EElementsViewMode.Icon;
                break;
            case EElementsViewMode.Icon:
                _playerData.SelectedMode = EElementsViewMode.Text;
                break;
        }
        SettingsUpdate?.Invoke();
    }

    public void Dispose()
    {

    }
}