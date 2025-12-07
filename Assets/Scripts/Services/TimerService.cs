using UnityEngine;
using Zenject;

public class TimerService : ITickable
{
    private readonly SignalBus _signalBus;

    private bool _isRunning = false;
    private float _timer = 0f;

    public float RemainingTime => _timer;

    public TimerService(
        SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Tick()
    {
        RunTimer();
    }

    private void RunTimer()
    {
        if (!_isRunning)
            return;

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _signalBus.Fire(new TimerEndSignal());
            StopTimer();
        }
    }

    public void StartTimer(float durationSeconds)
    {
        _timer = durationSeconds;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _timer = 0f;
        _isRunning = false;
    }
}