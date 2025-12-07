using System;

public class WinWindowModel : WindowModelBase
{
    public Action OnNext;

    public void Next()
    {
        OnNext?.Invoke();
    }
}