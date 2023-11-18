using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField] Image _imgIcon;
    [SerializeField] TMP_Text _textCount;

    [Header("Events")]
    [SerializeField] SelectItemUIEvent _selectItemEvent;

    public int ID { get { return _id; } }

    int _id;

    public void Setup(ItemData item)
    {
        _id = item.ID;
        _imgIcon.sprite = ConfigManager.Instance.GetItemConfig(item.ID).Icon;
        _textCount.text = item.Amount.ToString();
    }

    public void UpdateAmount(int amount)
    {
        _textCount.text = amount.ToString();
    }

    public void OnSelectItem()
    {
        _selectItemEvent?.RaiseSelectID(_id);
    }
}
