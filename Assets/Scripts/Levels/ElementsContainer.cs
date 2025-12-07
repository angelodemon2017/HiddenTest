using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementsContainer : MonoBehaviour
{
    [SerializeField] private List<SearchElement> _searchElements;

    private Dictionary<string, SearchElement> _mapElements = new();

    private Queue<string> _currentItems = new();

    public List<SearchElement> SearchElements => _searchElements;
    public bool IsLeftItems => _currentItems.Count > 0;

    public Action<string> OnElementClicked;

    [ContextMenu(nameof(FillElements))]
    private void FillElements()
    {
        _searchElements.Clear();
        var allElements = GetComponentsInChildren<SearchElement>();
        foreach (var elem in allElements)
        {
            _searchElements.Add(elem);
        }
    }

    public void StartLevelWithConfig(LevelFindObjectsConfig levelConfig)
    {
        _currentItems.Clear();
        foreach (var elem in _searchElements)
        {
            var conElem = levelConfig.GetConfigByElementId(elem.ElementID);
            elem.gameObject.SetActive(conElem.IsEnableElement);
            if (conElem.IsEnableElement)
            {
                elem.OnElementClicked += ClickElement;
                _currentItems.Enqueue(elem.ElementID);
            }
        }
    }

    private void ClickElement(string id)
    {
        OnElementClicked?.Invoke(id);
    }

    public void HideElement(string id)
    {
        var element = GetElementById(id);
        element.Hide();
    }

    public SearchElement GetElementById(string id)
    {
        if (!_mapElements.ContainsKey(id))
        {
            _mapElements.Clear();
            _searchElements.ForEach(e => _mapElements.Add(e.ElementID, e));
        }

        return _mapElements[id];
    }

    public string GetNextElementID()
    {
        if (_currentItems.Count == 0)
            return null;
        return _currentItems.Dequeue();
    }

    public void EndLevel()
    {
        _searchElements.ForEach(e => e.OnElementClicked = null);
    }
}