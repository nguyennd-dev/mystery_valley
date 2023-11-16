using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResource : MonoBehaviour
{
    public static GameResource Instance;

    void Awake()
    {
        Instance = this;
    }

    
}
