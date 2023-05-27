using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private  float _movementSpeed;
    [SerializeField, Range(0f,1f)] private  float _minimumMoveInput = .3f;
    
    private VirtualJoystick _joystick;
    private float _movementCameraMultiplier;
    
    public void Init()
    {
        if (GameManager.Instance.Joystick == null)
        {
            throw new Exception("MAKE SURE VIRTUAL JOYSTICK EXISTS");
        }
        if (GameManager.Instance.Camera == null)
        {
            throw new Exception("MAKE SURE CAMERA EXISTS");
        }
        
        _joystick = GameManager.Instance.Joystick;
        _movementCameraMultiplier = GameManager.Instance.Camera.transform.forward.z > 0 ? 1 : -1;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (_joystick.JoystickInput.magnitude > _minimumMoveInput)
        {
            transform.position = Vector3.MoveTowards(transform.position
                , transform.position + _joystick.JoystickInput * (_movementCameraMultiplier * 10),
                Time.deltaTime * _movementSpeed);
        }
    }
}
