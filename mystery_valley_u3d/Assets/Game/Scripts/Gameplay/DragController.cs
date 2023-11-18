using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour, IDragInput
{
    [SerializeField] GridController _gridController;
    [SerializeField] LayerMask _layerMaskDragable;
    [SerializeField] LayerMask _layerMaskBlock;

    [Header("Events")]
    [SerializeField] DragInputEvent _dragInput;
    [SerializeField] TapBlockEvent _tapBlockEvent;
    [SerializeField] SelectItemUIEvent _selectItemEvent;
    [SerializeField] PlaceItemEvent _placeItemEvent;
    [SerializeField] VoidEvent _mouseEnterUIEvent;
    [SerializeField] VoidEvent _mouseExitUIEvent;

    IGameController _gameController;

    int _currentItemID = -1;
    GBlock _dragItem = null;
    bool _isMouseOnUI = false;

    void Awake()
    {
        _dragInput.RegisterListener(this);
        _selectItemEvent.OnSelectItem += OnSelectItem;
        _mouseEnterUIEvent.OnEvent += OnMouseEnterUI;
        _mouseExitUIEvent.OnEvent += OnMouseExitUI;
    }

    void OnDestroy()
    {
        _dragInput.RemoveListener(this);
        _selectItemEvent.OnSelectItem -= OnSelectItem;
        _mouseEnterUIEvent.OnEvent += OnMouseEnterUI;
        _mouseExitUIEvent.OnEvent -= OnMouseExitUI;
    }

    public void Setup(GameController gameController)
    {
        _gameController = gameController;
    }

    private void OnSelectItem(int itemID)
    {
        _currentItemID = itemID;
    }

    public void OnBeginDrag(Vector2 screenPoint)
    {
        if (_currentItemID >= 0)
        {
            _dragItem = _gameController.GetItemByID(_currentItemID);
            _dragItem.transform.position = GetPointOnTerrian(screenPoint);
            _dragItem.transform.SetLayer(C.LayerConstants.LAYER_DRAG_BLOCK);
            _dragItem.gameObject.SetActive(!_isMouseOnUI);
        }
        else if (!_isMouseOnUI)
        {
            if (GetBlockByTap(screenPoint, out var block))
            {
                _tapBlockEvent?.RaiseTapBlock(block);
            }
        }
    }

    public void OnDrag(Vector2 screenPoint)
    {
        if (_currentItemID >= 0 && _dragItem != null)
        {
            _dragItem.gameObject.SetActive(!_isMouseOnUI);
            var pos = GetPointOnTerrian(screenPoint);
            _dragItem.transform.position = pos + new Vector3(0f, C.BlockConstants.HALF_SIZE_BLOCK, 0f);
            _gridController.EnableHighlight(!_isMouseOnUI, pos + new Vector3(C.BlockConstants.HALF_SIZE_BLOCK, 0, C.BlockConstants.HALF_SIZE_BLOCK));
        }
    }

    public void OnEndDrag(Vector2 screenPoint)
    {
        if (_currentItemID >= 0 && _dragItem != null)
        {
            if (!_isMouseOnUI)
                _placeItemEvent?.RaiseEvent(_currentItemID, _gridController.WorldToCell(_dragItem.transform.position - new Vector3(-C.BlockConstants.HALF_SIZE_BLOCK, 0, -C.BlockConstants.HALF_SIZE_BLOCK)));
            _gridController.EnableHighlight(false, Vector3.zero);
            _gameController.StoreItem(_currentItemID, _dragItem);
            _dragItem.transform.SetLayer(C.LayerConstants.LAYER_BLOCK);
            _currentItemID = -1;
            _dragItem = null;
        }
    }

    Vector3 GetPointOnTerrian(Vector2 screenPos)
    {
        Ray ray = _gameController.Camera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMaskDragable))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    bool GetBlockByTap(Vector2 screenPos, out GBlock tranformSelected)
    {
        tranformSelected = null;
        Ray ray = _gameController.Camera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMaskBlock))
        {
            tranformSelected = hit.collider.transform.GetComponent<GBlock>();
            return true;
        }
        return false;
    }

    void OnMouseEnterUI()
    {
        _isMouseOnUI = true;
    }

    void OnMouseExitUI()
    {
        _isMouseOnUI = false;
    }
}
