using System;

public class FailWindowModel : WindowModelBase
{
    public Action OnRepeat;

    public Action OnExit;

    public void Repeat()
    {
        OnRepeat?.Invoke();
    }

    public void Exit()
    {
        OnExit?.Invoke();
    }
}