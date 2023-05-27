using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Attacking,
        Moving,
    }
    
    [SerializeField] private  float _movementSpeed;
    [SerializeField] private  float _rotationSpeed;
    [SerializeField, Range(0f,1f)] private  float _minimumMoveInput = .3f;
    [SerializeField] private Animator _animator;
    
    private VirtualJoystick _joystick;
    private float _movementCameraMultiplier;

    private PlayerState _currentState;
    
    //animator keys
    private int _moveAnimationKey = Animator.StringToHash("IsMoving");
    
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

        _currentState = PlayerState.Idle;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (_joystick.JoystickInput.magnitude > _minimumMoveInput)
        {
            if (_currentState != PlayerState.Moving)
            {
                StartMoving();
            }

            transform.forward = Vector3.RotateTowards(transform.forward,
                _joystick.JoystickInput, _rotationSpeed * Time.deltaTime,
                1);
            transform.position = Vector3.MoveTowards(transform.position
                , transform.position + _joystick.JoystickInput * (_movementCameraMultiplier * 10),
                Time.deltaTime * _movementSpeed);
        }
        else
        {
            if (_currentState == PlayerState.Moving)
            {
                StopMoving();
            }
        }
    }

    private void StartMoving()
    {
        _currentState = PlayerState.Moving;
        _animator.SetBool(_moveAnimationKey, true);
    }

    private void StopMoving()
    {
        //TODO: CHECK HERE IF TARGET TO SET STATE TO IDLE OR ATTACKING
        _currentState = PlayerState.Idle;
        _animator.SetBool(_moveAnimationKey, false);
    }
}
