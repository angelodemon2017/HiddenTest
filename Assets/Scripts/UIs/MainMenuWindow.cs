using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : UIWindowBase<WindowModelBase>
{
    [SerializeField] private Button _startNewGameButton;

    public Action StartNewGameAction;

    private void Awake()
    {
        _startNewGameButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        StartNewGameAction?.Invoke();
    }

    private void OnDestroy()
    {
        _startNewGameButton.onClick.RemoveAllListeners();
    }
}