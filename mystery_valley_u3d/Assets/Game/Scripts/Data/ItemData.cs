using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int ID;
    public int Amount;

    public ItemData()
    {
        ID = 0;
        Amount = 0;
    }

    public ItemData(int id, int amount)
    {
        ID = id;
        Amount = amount;
    }
}
