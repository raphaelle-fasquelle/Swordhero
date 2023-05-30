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

    public void ResetGauge()
    {
        _currentLifeSlider.value = 1;
        _animationSlider.value = 1;
    }

    public void UpdateGauge(float targetValue)
    {
        DOTween.Kill(this);
        _currentLifeSlider.value = targetValue;
        _animationSlider.DOValue(targetValue, (_animationSlider.value - targetValue)/_animationSpeed).SetId(this);
    }
}
