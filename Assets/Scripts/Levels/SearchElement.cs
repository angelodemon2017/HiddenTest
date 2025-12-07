using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SearchElement : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private string _elementID;
    [SerializeField] private string _titleKey;
    [SerializeField] private Sprite _icon;

    public string ElementID => _elementID;
    public string Title => _titleKey;
    public Sprite Icon => _icon;

    public Action<string> OnElementClicked;

    private void OnValidate()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        OnElementClicked?.Invoke(_elementID);
    }

    public void Hide()
    {
        _boxCollider2D.enabled = false;
        _renderer.DOFade(0f, 1f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
    }
}