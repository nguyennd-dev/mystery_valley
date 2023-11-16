using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    public int Index;
    public int Value;
}

[System.Serializable]
public class GameData
{
    public List<SlotData> Slots;
    public Dictionary<int, int> Resources;

    public GameData()
    {
        Slots = new List<SlotData>();
        Resources = new Dictionary<int, int>();
    }
}
