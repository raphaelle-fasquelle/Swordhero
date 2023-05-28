using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private float _followOffset;
    private Transform _target;
    private Coroutine _updatePositionCrt;

    public void SetTarget(Transform target)
    {
        if(_target == target) return;
        gameObject.SetActive(true);
        _target = target;
        _updatePositionCrt = StartCoroutine(UpdatePositionCrt());
    }

    public void Disable()
    {
        if(_target == null) return;
        _target = null;
        StopCoroutine(_updatePositionCrt);
        gameObject.SetActive(false);
    }

    private IEnumerator UpdatePositionCrt()
    {
        while (_target != null)
        {
            transform.position = _target.position + _followOffset * Vector3.up;
            yield return null;
        }
    }


}
