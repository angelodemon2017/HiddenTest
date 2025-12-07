using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelFindObjectsConfig : LevelConfigBase
{
    [SerializeField] private ElementsContainer _elementsContainer;

    public string LevelId;
    [Range(1, 10)]
    public int CountElementsToFindTogether;
    public List<SearcherElementConfig> elementConfigs;

    private Dictionary<string, SearcherElementConfig> _mapConfigs = new();

    public ElementsContainer ElementsContainer => _elementsContainer;

    [ContextMenu(nameof(CalcElements))]
    public void CalcElements()
    {
        elementConfigs.Clear();
        foreach (var elem in _elementsContainer.SearchElements)
        {
            elementConfigs.Add(new SearcherElementConfig(elem.ElementID, elem.transform.position));
        }
    }

    public SearcherElementConfig GetConfigByElementId(string id)
    {
        if (!_mapConfigs.ContainsKey(id))
        {
            elementConfigs.ForEach(e => _mapConfigs.Add(e.Id, e));
        }

        return _mapConfigs[id];
    }
}