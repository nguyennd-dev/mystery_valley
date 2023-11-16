using System.Collections;
using System.Collections.Generic;
using StormStudio.Common;
using UnityEngine;
using static StormStudio.Common.GSMachine;

public partial class GameFlow : MonoBehaviour
{

    void GameState_Init(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            _controller = GameObject.Instantiate(_prefabWorld, Vector3.zero, Quaternion.identity).GetComponent<GameController>();
            _gsMachine.ChangeState(GameState.Gameplay);
        }
        else if (stateEvent == StateEvent.Exit)
        {
        }
    }

    public GameData CreateData()
    {
        var data = new GameData();
        return data;
    }
}
