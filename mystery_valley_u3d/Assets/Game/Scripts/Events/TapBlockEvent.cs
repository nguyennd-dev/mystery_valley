using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Tap Block")]
public class TapBlockEvent : ScriptableObject
{
    public event System.Action<GBlock> OnEvent;

    public void RaiseTapBlock(GBlock block)
    {
        OnEvent?.Invoke(block);
    }
}
