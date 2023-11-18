using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class QuitUI : UIController, IAnimator
{
    [SerializeField] TMP_Text _textCountdown;
    [SerializeField] TMP_Text _textCancel;

    Animator _animator;
    System.Action<bool> _onClose = null;
    Coroutine _crCountdown;
    int _countdown = 5;
    C.BtnType _btnType = C.BtnType.None;

    public void Setup(System.Action<bool> cb)
    {
        _animator = GetComponent<Animator>();
        _onClose = cb;
        _textCountdown.text = _countdown.ToString();
    }

    public void TouchedStop()
    {
        if (_btnType == C.BtnType.None)
        {
            SoundManager.Instance.PlaySfxTapButton();
            _btnType = C.BtnType.Close;
            _animator.Play("Close");
        }
    }

    public void OnShown()
    {
        _crCountdown = StartCoroutine(CRCountdown());

        Sequence seq = DOTween.Sequence();
        seq.Append(_textCancel.DOFade(1f, 1f));
        seq.AppendInterval(0.5f);
        seq.Append(_textCancel.DOFade(0f, 1f));
        seq.AppendInterval(0.2f);
        seq.SetLoops(-1);
    }

    IEnumerator CRCountdown()
    {
        while (_countdown > 0f)
        {
            _countdown -= 1;
            _textCountdown.text = _countdown.ToString();
            SoundManager.Instance.PlaySfxCountdown(_countdown == 0);
            var seq = DOTween.Sequence();
            seq.Append(_textCountdown.transform.DOScale(1.15f, 0.5f));
            seq.Append(_textCountdown.transform.DOScale(1.0f, 0.3f));
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.1f);
        if (_btnType == C.BtnType.None)
        {
            _btnType = C.BtnType.Quit;
            _animator.Play("Close");
        }
    }

    public void OnClosed()
    {
        switch (_btnType)
        {
            case C.BtnType.Quit:
                _onClose?.Invoke(false);
                break;

            case C.BtnType.Close:
                _onClose?.Invoke(true);
                break;
        }
        StartCoroutine(CRRelease());
    }

    IEnumerator CRRelease()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Instance.ReleaseUI(this, true);
    }
}
