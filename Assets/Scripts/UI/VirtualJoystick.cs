using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

[SelectionBase]
public class VirtualJoystick : MonoBehaviour
{
    public Vector3 JoystickInput => new Vector3(_knob.localPosition.x, 0, _knob.localPosition.y)  / _maxMagnitude;
    public bool HasInput => _hasInput;
    
    [SerializeField] private RectTransform _parentRect;
    [SerializeField] private RectTransform _knob;

    private Vector2 _initPos;
    private float _maxMagnitude;
    private bool _hasInput;
    
    public void Init()
    {
        _initPos = _parentRect.position;
        _maxMagnitude = _parentRect.sizeDelta.x / 2f - _knob.sizeDelta.x * 0.25f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ! IsAboveUi())
        {
            PointerDown(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            PointerUp(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && ! IsAboveUi())
        {
            Drag(Input.mousePosition);
        }
    }

    private void PointerDown(Vector3 position)
    {
        _parentRect.position = position;
        _hasInput = true;
    }

    private void PointerUp(Vector3 position)
    {
        _parentRect.position = _initPos;
        _knob.localPosition = Vector3.zero;
        _hasInput = false;
    }

    private void Drag(Vector3 position)
    {
        _knob.localPosition = Vector3.ClampMagnitude(position - _parentRect.position
            , _maxMagnitude);
    }
    
    private bool IsAboveUi() {
        return EventSystem.current.currentSelectedGameObject;
    }

}
