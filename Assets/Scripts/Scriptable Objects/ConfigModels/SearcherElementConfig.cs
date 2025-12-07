using UnityEngine;

[System.Serializable]
public class SearcherElementConfig
{
    [SerializeField] private string _id;
    [SerializeField] private bool _isEnableElement;
    [SerializeField] private Vector2 _position;

    public string Id => _id;
    public bool IsEnableElement => _isEnableElement;

    public SearcherElementConfig(string id, Vector2 position)
    {
        _id = id;
        _isEnableElement = true;
        _position = position;
    }
}