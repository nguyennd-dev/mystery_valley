using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragInput
{
    void OnBeginDrag(Vector2 screenPoint);
    void OnDrag(Vector2 screenPoint);
    void OnEndDrag(Vector2 screenPoint);
}

[CreateAssetMenu(menuName = "Events/Drag Input")]
public class DragInputEvent : ScriptableObject
{
    List<IDragInput> _listeners = null;

    public void RegisterListener(IDragInput listener)
    {
        if (_listeners == null) _listeners = new List<IDragInput>();
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void RemoveListener(IDragInput listener)
    {
        if (_listeners == null) _listeners = new List<IDragInput>();
        if (_listeners.Contains(listener))
            _listeners.Remove(listener);
    }

    public void RaiseBeginDrag(Vector2 screenPos)
    {
        foreach (var listener in _listeners)
            listener.OnBeginDrag(screenPos);
    }

    public void RaiseDrag(Vector2 screenPos)
    {
        foreach (var listener in _listeners)
            listener.OnDrag(screenPos);
    }

    public void RaiseEndDrag(Vector2 screenPos)
    {
        foreach (var listener in _listeners)
            listener.OnEndDrag(screenPos);
    }
}
