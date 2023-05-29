using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public PlayerWeapon PlayerWeapon => _playerWeapon;

    private enum PlayerState
    {
        Idle,
        Attacking,
        Moving,
    }

    //MOVEMENT
    [Header("Movement")]
    [SerializeField] private  float _rotationSpeed;
    [SerializeField, Range(0f,1f)] private  float _minimumMoveInput = .3f;
    [SerializeField] private float _baseSpeed;
    //ATACK
    [Space(10)]
    [Header("Attack")]
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _detectionAngle;
    [SerializeField] private int _attackDamage;
    //LINKS
    [Space(10)]
    [Header("Links")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerWeapon _playerWeapon;
    
    //EXTERNAL LINKS
    private VirtualJoystick _joystick;
    private EnemiesManager _enemiesManager;
    private TargetIndicator _targetIndicator;
    private CameraManager _cameraManager;
    
    //INTERNAL PARAMETERS
    private float _movementCameraMultiplier;
    private PlayerState _currentState;
    private EnemyController _targetEnemy;
    private bool _isStriking;
    private Coroutine _attackCrt;
    
    //WEAPON SHORTCUTS
    private WeaponDataSo _currentWeapon => GameManager.Instance.WeaponManager.CurrentWeapon;

    private bool _hasTarget => _targetEnemy != null;
    
    //animator keys
    private int _moveAnimationKey = Animator.StringToHash("IsMoving");
    private int _attackAnimationKey = Animator.StringToHash("Attack");
    
    public void Init()
    {
        _joystick = GameManager.Instance.Joystick;
        _enemiesManager = GameManager.Instance.EnemiesManager;
        _targetIndicator = GameManager.Instance.TargetIndicator;
        _cameraManager = GameManager.Instance.CameraManager;
        _movementCameraMultiplier = GameManager.Instance.Camera.transform.forward.z > 0 ? 1 : -1;

        _currentState = PlayerState.Idle;
    }

    private void Update()
    {
        UpdateMovement();
        if (_currentState == PlayerState.Attacking)
        {
            UpdateAttack();
        }
        else
        {
            CheckTarget();
        }

        if (_currentState == PlayerState.Idle)
        {
            if (_hasTarget)
            {
                UpdateState(PlayerState.Attacking);
            }
        }
    }

    #region MOVING
    private void UpdateMovement()
    {
        if (_joystick.HasInput)
        {
            if (_currentState != PlayerState.Moving)
            {
                UpdateState(PlayerState.Moving);
            } 
            if (_joystick.JoystickInput.magnitude > _minimumMoveInput)
            {
                transform.forward = Vector3.RotateTowards(transform.forward,
                    _joystick.JoystickInput * _movementCameraMultiplier, _rotationSpeed * Time.deltaTime,
                    1);
                transform.position = Vector3.MoveTowards(transform.position
                    , transform.position + _joystick.JoystickInput * (_movementCameraMultiplier * 10),
                    Time.deltaTime * _baseSpeed * _currentWeapon.MovementSpeedMultiplier);
            }
        }
        else
        {
            if (_currentState == PlayerState.Moving)
            {
                StopMoving();
            }
        }
    }

    private void StopMoving()
    {
        if (_hasTarget)
        {
            UpdateState(PlayerState.Attacking);
        }
        else
        {
            UpdateState(PlayerState.Idle);
        }
    }
    #endregion
    
    #region ATTACK

    private void UpdateAttack()
    {
        if(_isStriking) return;
        if (!_targetEnemy.IsAlive)
        {
            _targetIndicator.Disable();
            _targetEnemy = null;
            UpdateState(PlayerState.Idle);
        }
        else
        {
            Vector3 direction = (_targetEnemy.transform.position - transform.position).normalized;
            transform.forward = Vector3.RotateTowards(transform.forward,direction, _rotationSpeed * Time.deltaTime, 1);
            if (IsInRange(_targetEnemy.transform.position, _currentWeapon.AttackRange))
            {
                DealAttack();
            }
            else
            {
                _animator.SetBool(_moveAnimationKey, true);
                transform.position = Vector3.MoveTowards(transform.position
                    , _targetEnemy.transform.position - 1f * direction,
                    Time.deltaTime * _baseSpeed * _currentWeapon.MovementSpeedMultiplier);
            }
        }
    }

    private void DealAttack()
    {
        SetAnimatorSpeed(_currentWeapon.AttackSpeedMultiplier);
        _animator.SetBool(_moveAnimationKey, false);
        _animator.SetTrigger(_attackAnimationKey);
        _attackCrt = StartCoroutine(AttackCrt());
    }

    private IEnumerator AttackCrt()
    {
        _isStriking = true;
        yield return new WaitForSeconds(_currentWeapon.TimingToHitEffect);
        DoHit();
        yield return new WaitForSeconds(_currentWeapon.EndAttackCooldown);
        _isStriking = false;
    }

    private void DoHit()
    {
        _targetEnemy.TakeDamage(_attackDamage);
        _cameraManager.DoShake(_currentWeapon.ShakeAmplitude,_currentWeapon.ShakeIntensity,_currentWeapon.ShakeDuration);
    }

    private void CancelAttack()
    {
        StopCoroutine(_attackCrt);
        _isStriking = false;
    }
    
    #endregion

    private void UpdateState(PlayerState state)
    {
        if(_currentState == state) return;
        switch (state)
        {
            case PlayerState.Idle:
                _currentState = PlayerState.Idle;
                _animator.SetBool(_moveAnimationKey, false);
                SetAnimatorSpeed(1f);
                break;
            case PlayerState.Moving:
                if (_currentState == PlayerState.Attacking && _isStriking)
                {
                    CancelAttack();
                }
                SetAnimatorSpeed(_currentWeapon.MovementSpeedMultiplier);
                _currentState = PlayerState.Moving;
                _animator.SetBool(_moveAnimationKey, true);
                break;
            case PlayerState.Attacking:
                _currentState = PlayerState.Attacking;
                break;
        }
    }

    private void SetAnimatorSpeed(float speed)
    {
        Debug.Log("Change animator speed to "+speed);
        _animator.speed = speed;
    }

    private void CheckTarget()
    {
        _targetEnemy = _enemiesManager.GetClosestAliveEnemyInConeRange(transform, _detectionAngle, _currentWeapon.AttackRange);
        if (_hasTarget)
        {
            _targetIndicator.SetTarget(_targetEnemy.transform);
        }
        else
        {
            _targetIndicator.Disable();
        }
    }

    private bool IsInRange(Vector3 position, float range)
    {
        return Vector3.Distance(position, transform.position) <= range;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + _detectionRange * (Quaternion.AngleAxis(_detectionAngle/2, Vector3.up) * transform.forward));
        Gizmos.DrawLine(transform.position, transform.position + _detectionRange * (Quaternion.AngleAxis(-_detectionAngle/2, Vector3.up) * transform.forward));

        if (GameManager.Instance != null && GameManager.Instance.WeaponManager.CurrentWeapon != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _currentWeapon.AttackRange);
        }
    }
    #endif
}
