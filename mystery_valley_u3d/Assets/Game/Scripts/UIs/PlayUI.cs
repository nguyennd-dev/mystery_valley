using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using System;

public class PlayUI : UIController
{
    [Header("UI")]
    [SerializeField] List<Transform> _tfSlots;
    [SerializeField] UIItem _prefabItem;

    [Header("Data")]
    [SerializeField] ResourceDataSO _resourceData;

    ObjectPool<UIItem> _poolItems;

    System.Action _onPause = null;
    List<UIItem> _items = new List<UIItem>();

    void OnEnable()
    {
        _resourceData.OnDataChanged += OnResourceChanged;
    }

    void OnDisable()
    {
        _resourceData.OnDataChanged -= OnResourceChanged;
    }

    public void Setup(System.Action onPause)
    {
        _onPause = onPause;
        _poolItems = new ObjectPool<UIItem>(_prefabItem);
        foreach (var data in _resourceData.Items)
        {
            if (data.Amount <= 0) continue;
            var itemUI = _poolItems.Get();
            itemUI.Setup(data);
            itemUI.transform.SetParent(_tfSlots[_items.Count]);
            itemUI.transform.localPosition = Vector2.zero;
            _items.Add(itemUI);
        }
    }

    void OnResourceChanged(ResourceDataSO resourceData)
    {
        var newItems = new List<UIItem>();
        foreach (var data in _resourceData.Items)
        {
            if (data.Amount <= 0) continue;
            var itemUI = _items.Find(val => val.ID == data.ID);
            if (itemUI == null)
            {
                itemUI = _poolItems.Get();
                itemUI.Setup(data);
            }
            else
                itemUI.UpdateAmount(data.Amount);
            itemUI.transform.SetParent(_tfSlots[newItems.Count]);
            itemUI.transform.localPosition = Vector2.zero;
            newItems.Add(itemUI);
        }

        var removeItems = new List<UIItem>();
        foreach (var lastItem in _items)
        {
            if (newItems.Find(val => val.ID == lastItem.ID)) continue;
            removeItems.Add(lastItem);
        }
        _items.Clear();
        _items = newItems;

        foreach (var item in removeItems)
            _poolItems.Store(item);
    }

    public void TouchedHelp()
    {
        SoundManager.Instance.PlaySfxTapButton();
        var helpUI = UIManager.Instance.ShowUIOnTop<HelpUI>("HelpUI");
        helpUI.Setup(() => { });
    }

    public void TouchedPause()
    {
        SoundManager.Instance.PlaySfxTapButton();
        _onPause?.Invoke();
    }
}
