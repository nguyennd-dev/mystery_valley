using System.Collections;
using System.Collections.Generic;
using StormStudio.Common;
using UnityEngine;
using static StormStudio.Common.GSMachine;

public partial class GameFlow : MonoBehaviour
{
    [Header("World")]
    [SerializeField] GameObject _prefabWorld;

    GameController _controller;

    void GameState_Gameplay(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            
        }
        else if (stateEvent == StateEvent.Exit)
        {
        }
    }
}
