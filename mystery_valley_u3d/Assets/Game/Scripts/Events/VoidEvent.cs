using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEvent : ScriptableObject
{
    public event System.Action OnEvent;

    public void RaiseEvent()
    {
        OnEvent?.Invoke();
    }
}
