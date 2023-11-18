using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SplashUI : UIController
{
    [SerializeField] Slider _sliderLoading;
    [SerializeField] TMP_Text _textLoading;

    public void Setup(System.Action cb)
    {
        _sliderLoading.value = 0;
        _sliderLoading.DOValue(100f, 3f).OnComplete(() => cb?.Invoke());
    }

    public void OnLoadChanged()
    {
        _textLoading.text = $"{_sliderLoading.value:00}%";
    }
}
