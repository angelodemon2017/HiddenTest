using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FindingElementViewUI : MonoBehaviour, IPoolable<IMemoryPool>
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameElement;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;

    private IMemoryPool _pool;
    private DataService _dataService;

    [Inject]
    public void Constructor(
        DataService dataService)
    {
        _dataService = dataService;
        UpdateView();
        _dataService.SettingsUpdate += UpdateView;
    }

    public void UpdateView(SearchElement searchElement)
    {
        _icon.sprite = searchElement.Icon;
        _nameElement.text = searchElement.Title;
    }

    private void UpdateView()
    {
        ApplyMode(_dataService.CurrentElementsViewMode);
    }

    private void ApplyMode(EElementsViewMode elementsViewMode)
    {
        _aspectRatioFitter.aspectRatio = elementsViewMode == EElementsViewMode.Icon ? 1 : 2;
        _icon.gameObject.SetActive(elementsViewMode == EElementsViewMode.Icon);
        _nameElement.gameObject.SetActive(elementsViewMode == EElementsViewMode.Text);
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public void Show()
    {
        Vector3 targetScale = transform.localScale;

        transform.localScale = Vector3.zero;

        transform.DOScale(targetScale, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            });
    }

    public void OnDespawned()
    {

    }

    public void ReturnToPool()
    {
        _pool.Despawn(this);
    }

    public class Pool : MemoryPool<FindingElementViewUI>
    {
        protected override void OnSpawned(FindingElementViewUI item)
        {
            base.OnSpawned(item);
            item.gameObject.SetActive(true);
            item.Show();
        }

        protected override void OnDespawned(FindingElementViewUI item)
        {
            item.gameObject.SetActive(false);
            base.OnDespawned(item);
        }
    }

    public class Factory : PlaceholderFactory<FindingElementViewUI>
    {
    }

    private void OnDestroy()
    {
        _dataService.SettingsUpdate -= UpdateView;
    }
}