using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using System;
using UnityEngine.UI;

public class PauseUI : UIController, IAnimator
{
    [Header("UI")]
    [SerializeField] private Toggle _toggleSound;
    [SerializeField] private Toggle _toggleMusic;

    public System.Action OnRestart;
    public System.Action OnResume;
    public System.Action OnQuit;

    private Animator _animator;
    C.BtnType _type = C.BtnType.None;

    public void Setup(bool enabledSound, bool enableMusic)
    {
        _animator = GetComponent<Animator>();
        _toggleSound.SetIsOnWithoutNotify(enabledSound);
        _toggleMusic.SetIsOnWithoutNotify(enableMusic);
    }

    public void OnShown() { }

    public void OnClosed()
    {
        StartCoroutine(CRRelease());
        switch (_type)
        {
            case C.BtnType.Quit:
                OnQuit?.Invoke();
                break;

            case C.BtnType.Restart:
                OnRestart?.Invoke();
                break;

            default:
                OnResume?.Invoke();
                break;
        }
    }

    IEnumerator CRRelease()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Instance.ReleaseUI(this, true);
    }

    public void TouchedResume()
    {
        SoundManager.Instance.PlaySfxTapButton();
        _animator.Play("Close");
        _type = C.BtnType.None;
    }

    public void TouchedRestart()
    {
        SoundManager.Instance.PlaySfxTapButton();
        _animator.Play("Close");
        _type = C.BtnType.Restart;
    }

    public void TouchedQuit()
    {
        SoundManager.Instance.PlaySfxTapButton();
        _animator.Play("Close");
        _type = C.BtnType.Quit;
    }

    public void TouchedSound()
    {
        SoundManager.Instance.PlaySfxTapButton();
        SoundManager.Instance.ToggleSfx();
    }

    public void TouchedMusic()
    {
        SoundManager.Instance.PlaySfxTapButton();
        SoundManager.Instance.ToggleBGM();
    }
}