using UnityEngine;
using Zenject;

public class FailAppState : AppStateWithUIBase<FailWindow, FailWindowModel>
{
    [Inject] private LevelsSequence _levelsSequence;
    [Inject] private DataService _dataService;

    public FailAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService) { }

    protected override void SubscribeUICallBacks()
    {
        _model.OnRepeat += Repeat;
        _model.OnExit += Quit;
    }

    private void Repeat()
    {
        _signalBus.Fire(
            new RunLevelSignal(
                _levelsSequence.GetConfigByIndex(_dataService.CurrentLevel)));
    }

    private void Quit()
    {
        Application.Quit();
    }

    protected override void UnSubscribeUICallBacks()
    {
        _model.OnRepeat -= Repeat;
        _model.OnExit -= Quit;
    }
}