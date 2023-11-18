using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Place Item")]
public class PlaceItemEvent : ScriptableObject
{
    public event System.Action<int, Vector3Int> OnEvent;

    public void RaiseEvent(int id, Vector3Int point)
    {
        OnEvent?.Invoke(id, point);
    }
}
