using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour, IDragInput
{
    [Header("Events")]
    [SerializeField] DragInputEvent _dragInput;

    void Awake()
    {
        _dragInput.RegisterListener(this);
    }

    void OnDestroy()
    {
        _dragInput.RemoveListener(this);
    }

    public void OnBeginDrag(Vector2 screenPoint)
    {
        Debug.LogError("Begin Drag: " + screenPoint);
    }

    public void OnDrag(Vector2 screenPoint)
    {
        Debug.LogError("Dragging: " + screenPoint);
    }

    public void OnEndDrag(Vector2 screenPoint)
    {
        Debug.LogError("End Drag: " + screenPoint);
    }
}
