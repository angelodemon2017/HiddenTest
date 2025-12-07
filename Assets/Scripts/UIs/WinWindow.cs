using UnityEngine;
using UnityEngine.UI;

public class WinWindow : UIWindowBase<WinWindowModel>
{
    [SerializeField] private Button _winButton;

    public override void Show()
    {
        base.Show();
        _winButton.onClick.AddListener(OnRepeatButtonClicked);
    }

    private void OnRepeatButtonClicked()
    {
        _model.Next();
    }

    public override void Hide()
    {
        base.Hide();
        _winButton.onClick.RemoveListener(OnRepeatButtonClicked);
        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}