using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    [SerializeField] private KeyCode _escapeKey = KeyCode.Escape;

    public Action EscapeAction;

    private void Update()
    {
        if (Input.GetKeyDown(_escapeKey))
        {
            EscapeAction?.Invoke();
        }
    }
}