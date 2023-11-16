using System.Collections;
using System.Collections.Generic;
using StormStudio.Common;
using UnityEngine;

public partial class GameFlow : MonoBehaviour
{
    public enum GameState
    {
        Init,
        Gameplay
    }

    public static GameFlow Instance { get; private set; }

    private GSMachine _gsMachine = new GSMachine();

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

#if UNITY_STANDALONE && !UNITY_EDITOR
        Screen.SetResolution(1280, 720, false);
#endif

        if (Application.isEditor)
            Application.runInBackground = true;

        Application.targetFrameRate = 60;
    }

    IEnumerator Start()
    {
        _gsMachine.Init(OnStateChanged, GameState.Init);
        while (true)
        {
            _gsMachine.StateUpdate();
            yield return null;
        }
    }

    #region GSMachine
    GSMachine.UpdateStateDelegate OnStateChanged(System.Enum state)
    {
        switch (state)
        {
            case GameState.Init:
                return GameState_Init;

            case GameState.Gameplay:
                return GameState_Gameplay;
        }

        return null;
    }
    #endregion
}
