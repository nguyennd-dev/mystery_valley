using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void SetLayer(this Transform transform, string layerName)
    {
        var layerMask = LayerMask.NameToLayer(layerName);
        transform.gameObject.gameObject.layer = layerMask;
        foreach (Transform child in transform)
        {
            SetLayer(child, layerName);
        }
    }
}