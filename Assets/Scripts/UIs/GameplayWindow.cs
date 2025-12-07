using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class GameplayWindow : UIWindowBase<FindingElementsModel>
{
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Transform _transformParentOfElements;

    private Dictionary<string, FindingElementViewUI> _cashElementsUI = new();

    private SignalBus _signalBus;
    private TimerService _timerService;
    private IFindingElementViewUIFactory _factory;

    [Inject]
    public void Constructor(
        SignalBus signalBus,
        TimerService timerService,
        IFindingElementViewUIFactory factory)
    {
        _signalBus = signalBus;
        _timerService = timerService;
        _factory = factory;
    }

    public override void Show()
    {
        base.Show();
        UpdateListOfElements();
        UpdateTimerPanel();
        _signalBus.Subscribe<AddElementSignal>(Handle);
        _signalBus.Subscribe<RemoveElementSignal>(Handle);
        _signalBus.Subscribe<TimerEndSignal>(Handle);
    }

    private void UpdateListOfElements()
    {
        foreach (var elem in _cashElementsUI)
        {
            _factory.Despawn(elem.Value);
        }
        _cashElementsUI.Clear();
        foreach (var element in _model.IdsForSearching)
        {
            AddViewElement(element);
        }
        Vector3 currentScale = _transformParentOfElements.localScale;
        _transformParentOfElements.localScale = new Vector3(currentScale.x, 0f, currentScale.z);
        _transformParentOfElements.DOScaleY(1f, 0.1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                foreach (var elem in _cashElementsUI)
                {
                    elem.Value.gameObject.SetActive(false);
                    elem.Value.gameObject.SetActive(true);
                }
                _transformParentOfElements.localScale = currentScale;
            });
    }

    private void Handle(AddElementSignal addElement)
    {
        AddViewElement(addElement.ElementID);
    }

    private void Handle(RemoveElementSignal removeElement)
    {
        RemoveViewElement(removeElement.ElementID);
    }

    private void Handle(TimerEndSignal timerEnd)
    {
        UpdateTimerPanel();
    }

    private void AddViewElement(string id)
    {
        if (!_cashElementsUI.ContainsKey(id))
        {
            var fevui = _factory.Create();
            fevui.transform.SetParent(_transformParentOfElements);
            fevui.UpdateView(_model.ElementsContainer.GetElementById(id));
            _cashElementsUI.Add(id, fevui);
        }
    }

    private void RemoveViewElement(string id)
    {
        if (_cashElementsUI.ContainsKey(id))
        {
            var viewUI = _cashElementsUI[id];
            _cashElementsUI.Remove(id);
            _factory.Despawn(viewUI);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_timerService.RemainingTime > 0)
        {
            _timerText.text = $"{(int)_timerService.RemainingTime}s";
        }
    }

    private void UpdateTimerPanel()
    {
        _timerPanel.SetActive(_timerService.RemainingTime > 0);
    }

    public override void Hide()
    {
        base.Hide();

        _signalBus.Unsubscribe<AddElementSignal>(Handle);
        _signalBus.Unsubscribe<RemoveElementSignal>(Handle);
        _signalBus.Unsubscribe<TimerEndSignal>(Handle);
    }
}