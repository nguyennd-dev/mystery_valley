using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector3Int Position;
    public int Value;

    public TileData()
    {
        Position = Vector3Int.zero;
        Value = -1;
    }

    public TileData(int id, Vector3Int point)
    {
        Position = point;
        Value = id;
    }
}

[System.Serializable]
public class GameData
{
    public List<TileData> Slots;
    public Dictionary<int, int> Resources;

    public GameData()
    {
        Slots = new List<TileData>();
        Resources = new Dictionary<int, int>();
    }
}
