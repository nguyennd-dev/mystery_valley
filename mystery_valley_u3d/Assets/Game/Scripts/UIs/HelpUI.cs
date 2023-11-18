using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using System;
using UnityEngine.UI;

public class HelpUI : UIController, IAnimator
{
    System.Action _onClose;
    Animator _animator;

    public void Setup(System.Action onClose)
    {
        _animator = GetComponent<Animator>();
        _onClose = onClose;
    }

    public void OnShown() { }

    public void OnClosed()
    {
        _onClose?.Invoke();
        StartCoroutine(CRRelease());
    }

    IEnumerator CRRelease()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Instance.ReleaseUI(this, true);
    }

    public void TouchedClose()
    {
        SoundManager.Instance.PlaySfxTapButton();
        _animator.Play("Close");
    }
}