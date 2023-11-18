using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Select Item UI")]
public class SelectItemUIEvent : ScriptableObject
{
    public event System.Action<int> OnSelectItem;

    public void RaiseSelectID(int id)
    {
        OnSelectItem?.Invoke(id);
    }
}
