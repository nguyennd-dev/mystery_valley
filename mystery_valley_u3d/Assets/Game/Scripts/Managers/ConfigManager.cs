using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemConfig
{
    public int ID;
    public Sprite Icon;
    public GameObject Prefab;
}

public class ConfigManager : MonoBehaviour
{
    [SerializeField] List<ItemConfig> _itemConfigs;

    public static ConfigManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public List<int> GetAllItemIDs()
    {
        List<int> ids = new List<int>();
        foreach (var item in _itemConfigs)
            ids.Add(item.ID);
        return ids;
    }

    public ItemConfig GetItemConfig(int id)
    {
        return _itemConfigs.Find(val => val.ID == id);
    }
}
