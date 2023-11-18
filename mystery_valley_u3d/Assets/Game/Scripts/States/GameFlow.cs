using System.Collections;
using System.Collections.Generic;
using StormStudio.Common;
using StormStudio.Common.UI;
using StormStudio.Common.Utils;
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

    public void SceneTransition(System.Action onSceneOutFinished)
    {
        UIManager.Instance.SetUIInteractable(false);
        SceneDirector.Instance.Transition(new TransitionFade()
        {
            duration = 0.667f,
            tweenIn = TweenFunc.TweenType.Sine_EaseInOut,
            tweenOut = TweenFunc.TweenType.Sine_EaseOut,
            onStepOutDidFinish = () =>
            {
                onSceneOutFinished.Invoke();
            },
            onStepInDidFinish = () =>
            {
                UIManager.Instance.SetUIInteractable(true);
            }
        });
    }

    IEnumerator Start()
    {
        SoundManager.Instance.LoadSoundSettings();
        SoundManager.Instance.OnEnableMusic += onEnableMusic;

        _gsMachine.Init(OnStateChanged, GameState.Init);
        while (true)
        {
            _gsMachine.StateUpdate();
            yield return null;
        }
    }

    private void onEnableMusic(bool enabled)
    {
        if (enabled)
        {
            switch ((GameState)_gsMachine.CurrentState)
            {
                case GameState.Gameplay:
                    SoundManager.Instance.PlayBgmGameplay();
                    break;
            }
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
