using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/World Data")]
public class WorldDataSO : ScriptableObject
{
    public List<TileData> Tiles = new List<TileData>();

    public event System.Action<WorldDataSO> OnDataChanged;

    public void RaiseChangeEvent()
    {
        OnDataChanged?.Invoke(this);
    }

    public void AddTile(TileData newTile)
    {
        Tiles.Add(newTile);
        RaiseChangeEvent();
    }

    public void LoadWorld(string jsonData)
    {
        if (!string.IsNullOrEmpty(jsonData))
            Tiles = JsonConvert.DeserializeObject<List<TileData>>(jsonData);
        else
            Tiles.Clear();
        RaiseChangeEvent();
    }

    public void RemoveTile(Vector3Int pos)
    {
        Tiles.RemoveAll(val => val.Position == pos);
        RaiseChangeEvent();
    }
}
