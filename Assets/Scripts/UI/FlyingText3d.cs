using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FlyingText3d : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    private Sequence _flySequence;
    
    public void DoFly(string value, Vector3 startPos, Color color, Action completionCallback)
    {
        transform.position = startPos;
        _text.SetText(value);
        _text.color = color;
        _text.alpha = 0;
        transform.localScale = Vector3.zero;
        DoFlySequence(completionCallback);
    }

    private void DoFlySequence(Action completionCallback)
    {
        if(_flySequence != null) _flySequence.Kill(true);
        
        _flySequence = DOTween.Sequence();
        _flySequence.Append(_text.DOFade(1, .15f));
        _flySequence.Join(transform.DOScale(1f, .25f));
        _flySequence.AppendInterval(.5f);
        _flySequence.Append(_text.DOFade(0, .3f));
        _flySequence.Join(transform.DOScale(0, .3f));
        _flySequence.OnComplete(completionCallback.Invoke);
    }
}
