using Zenject;

public class LoadingAppState : AppStateWithUIBase<LoadingWindow, LoadingWindowModel>
{
    public LoadingAppState(SignalBus signalBus, InputService inputService) :
        base(signalBus, inputService)
    {

    }
}