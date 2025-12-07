using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsSequence", menuName = "Scriptable Objects/LevelsSequence")]
public class LevelsSequence : ScriptableObject
{
    [SerializeField] private LevelsConfig _configsForSearchingItems;
    [SerializeField] private List<string> _sequence;

    private Dictionary<string, LevelsConfig> _mapConfigs = new();

    public int CountOfLevels => _sequence.Count;

    private void OnValidate()
    {
        foreach (var conf in _configsForSearchingItems.LevelConfigs)
        {
            HashSet<string> uniqLevels = new();
            if (!uniqLevels.Add(conf.LevelId))
            {
                Debug.LogError($"Level ID '{conf.LevelId}' is duplicated in LevelsSequence '{name}'");
            }
            if (string.IsNullOrEmpty(conf.LevelId))
            {
                Debug.LogError($"ConfigsForSearchingItems contain config with empty id");
            }
        }
        if (_sequence
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList().Any())
        {
            Debug.LogError($"LevelsSequence '{name}' contain duplicated level IDs in sequence");
        }
    }

    [ContextMenu(nameof(LoadDefSequence))]
    public void LoadDefSequence()
    {
        _sequence.Clear();
        _configsForSearchingItems.LevelConfigs.ForEach(l => _sequence.Add(l.LevelId));
    }

    public string GetSceneIdByIndex(int index)
    {
        string id = _sequence[index];
        return GetBundleConfigByLevelId(id).SceneName;
    }

    public LevelConfigBase GetConfigByIndex(int index)
    {
        string id = _sequence[index];
        return GetConfigById(id);
    }

    private LevelConfigBase GetConfigById(string id)
    {
        return GetBundleConfigByLevelId(id).GetConfigById(id);
    }

    private LevelsConfig GetBundleConfigByLevelId(string id)
    {
        if (!_mapConfigs.ContainsKey(id))
        {
            _mapConfigs.Clear();
            _configsForSearchingItems.LevelConfigs.ForEach(l => _mapConfigs.Add(l.LevelId, _configsForSearchingItems));
        }

        return _mapConfigs[id];
    }
}