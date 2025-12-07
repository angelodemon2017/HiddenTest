using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuWindow : UIWindowBase<WindowModelBase>
{
    [SerializeField] private Button _changeModeButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TextMeshProUGUI _modeLabel;

    private DataService _dataService;

    [Inject]
    public void Constructor(
        DataService dataService)
    {
        _dataService = dataService;

        _changeModeButton.onClick.AddListener(ChangeMode);
        _quitButton.onClick.AddListener(Quit);
    }

    public override void Show()
    {
        base.Show();
        _dataService.SettingsUpdate += UpdateView;
        UpdateView();
    }

    private void ChangeMode()
    {
        _dataService.ChangeViewMode();
    }

    private void UpdateView()
    {
        _modeLabel.text = $"Mode:{_dataService.CurrentElementsViewMode}";
    }

    private void Quit()
    {
        Application.Quit();
    }

    public override void Hide()
    {
        base.Hide();

        _dataService.SettingsUpdate -= UpdateView;
    }

    private void OnDestroy()
    {
        _changeModeButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }
}