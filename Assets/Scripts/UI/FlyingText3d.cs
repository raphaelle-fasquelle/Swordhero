using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FlyingText3d : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    public void DoFly(string value, Vector3 startPos, Color color)
    {
        transform.position = startPos;
        _text.SetText(value);
        _text.color = color;
        _text.alpha = 0;
        transform.localScale = Vector3.zero;
        Sequence flySequence = DOTween.Sequence();
        flySequence.Append(_text.DOFade(1, .15f));
        flySequence.Join(transform.DOScale(1f, .25f));
        flySequence.AppendInterval(.5f);
        flySequence.Append(_text.DOFade(0, .3f));
        flySequence.Join(transform.DOScale(0, .3f));
        flySequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
