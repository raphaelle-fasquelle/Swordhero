using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{
    [SerializeField] private Slider _currentLifeSlider;
    [SerializeField] private Slider _animationSlider;
    [SerializeField] private float _animationSpeed = 10f;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void ResetGauge()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.alpha = 1;
        _currentLifeSlider.value = 1;
        _animationSlider.value = 1;
    }

    public void UpdateGauge(float targetValue)
    {
        DOTween.Kill(this);
        _currentLifeSlider.value = targetValue;
        _animationSlider.DOValue(targetValue, (_animationSlider.value - targetValue)/_animationSpeed)
            .SetId(this)
            .OnComplete(() =>
            {
                if(targetValue <= 0) FadeOut();
            });
    }

    private void FadeOut()
    {
        DOTween.Kill(_canvasGroup, true);
        _canvasGroup.DOFade(0,.3f).SetId(_canvasGroup);
    }
}
