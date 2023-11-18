using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBlock : MonoBehaviour
{
    public int ID { get { return _id; } }
    int _id;

    public void Setup(int id)
    {
        _id = id;
    }
}
