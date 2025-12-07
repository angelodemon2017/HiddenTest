using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Scriptable Objects/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneForMode;

    private void OnValidate()
    {
        SceneName = _sceneForMode.name;
    }
#endif
    public List<LevelFindObjectsConfig> LevelConfigs;

    private Dictionary<string, LevelFindObjectsConfig> _mapLevels = new();

    public string SceneName;

    public LevelConfigBase GetConfigById(string id)
    {
        if (!_mapLevels.ContainsKey(id))
        {
            _mapLevels.Clear();
            LevelConfigs.ForEach(l => _mapLevels.Add(l.LevelId, l));
        }

        return _mapLevels[id];
    }


    [ContextMenu(nameof(CalcLevel))]
    public void CalcLevel()
    {
        LevelConfigs.ForEach(l => l.CalcElements());
    }
}