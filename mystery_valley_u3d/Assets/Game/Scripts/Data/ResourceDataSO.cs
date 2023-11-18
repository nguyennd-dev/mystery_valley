using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Resource Data")]
public class ResourceDataSO : ScriptableObject
{
    public List<ItemData> Items;

    public event System.Action<ResourceDataSO> OnDataChanged;

    public void RaiseChangeEvent()
    {
        OnDataChanged?.Invoke(this);
    }

    public void LoadResource(string jsonData)
    {
        if (Items == null) Items = new List<ItemData>();
        Items = JsonConvert.DeserializeObject<List<ItemData>>(jsonData);
        RaiseChangeEvent();
    }

    public void AddResource(int id, int amount)
    {
        var itemData = Items.Find(val => val.ID == id);
        if (itemData != null)
        {
            itemData.Amount += amount;
        }
        else
        {
            Items.Add(new ItemData(id, amount));
        }
        RaiseChangeEvent();
    }

    public bool ConsumeResource(int id, int amount)
    {
        var itemData = Items.Find(val => val.ID == id);
        if (itemData != null)
        {
            if (itemData.Amount >= amount)
            {
                itemData.Amount -= amount;
                RaiseChangeEvent();
                return true;
            }
        }
        return false;
    }
}
