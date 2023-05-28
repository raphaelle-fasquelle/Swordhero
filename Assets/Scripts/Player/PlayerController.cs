using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Attacking,
        Moving,
    }
    
    //MOVEMENT
    [SerializeField] private  float _movementSpeed;
    [SerializeField] private  float _rotationSpeed;
    [SerializeField, Range(0f,1f)] private  float _minimumMoveInput = .3f;
    //ATACK
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _detectionAngle;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackRange; //TODO: MOVE TO WEAPONS DATA
    [SerializeField] private int _attackDamage; //TODO: MOVE TO WEAPONS DATA
    [SerializeField] private float _animationHitDelay; //TODO: MOVE TO WEAPONS DATA
    [SerializeField] private float _endAttackCooldown;
    //LINKS
    [SerializeField] private Animator _animator;
    
    //EXTERNAL LINKS
    private VirtualJoystick _joystick;
    private EnemiesManager _enemiesManager;
    private TargetIndicator _targetIndicator;
    
    //INTERNAL PARAMETERS
    private float _movementCameraMultiplier;
    private PlayerState _currentState;
    private EnemyController _targetEnemy;
    private bool _isStriking;
    private Coroutine _attackCrt;

    private bool _hasTarget => _targetEnemy != null;
    
    //animator keys
    private int _moveAnimationKey = Animator.StringToHash("IsMoving");
    private int _attackAnimationKey = Animator.StringToHash("Attack");
    
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
        if (GameManager.Instance.EnemiesManager == null)
        {
            throw new Exception("MAKE SURE ENEMIES MANAGER EXISTS");
        }
        if (GameManager.Instance.TargetIndicator == null)
        {
            throw new Exception("MAKE SURE TARGET INDICATOR EXISTS");
        }
        
        _joystick = GameManager.Instance.Joystick;
        _enemiesManager = GameManager.Instance.EnemiesManager;
        _targetIndicator = GameManager.Instance.TargetIndicator;
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
        //TODO: IF IDLE AND ENEMY BECOMES IN RANGE, CHANGE STATE TO ATTACK
    }

    #region MOVING
    private void UpdateMovement()
    {
        if (_joystick.HasInput)
        {
            if (_currentState != PlayerState.Moving)
            {
                StartMoving();
            } 
            if (_joystick.JoystickInput.magnitude > _minimumMoveInput)
            {
                transform.forward = Vector3.RotateTowards(transform.forward,
                    _joystick.JoystickInput * _movementCameraMultiplier, _rotationSpeed * Time.deltaTime,
                    1);
                transform.position = Vector3.MoveTowards(transform.position
                    , transform.position + _joystick.JoystickInput * (_movementCameraMultiplier * 10),
                    Time.deltaTime * _movementSpeed);
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

    private void StartMoving()
    {
        UpdateState(PlayerState.Moving);
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
            if (IsInRange(_targetEnemy.transform.position, _attackRange))
            {
                DealAttack();
            }
            else
            {
                Vector3 direction = (_targetEnemy.transform.position - transform.position).normalized;
                transform.forward = Vector3.RotateTowards(transform.forward,direction, _rotationSpeed * Time.deltaTime, 1);
                transform.position = Vector3.MoveTowards(transform.position
                    , _targetEnemy.transform.position - 1f * direction,
                    Time.deltaTime * _attackSpeed);
            }
        }
    }

    private void DealAttack()
    {
        _animator.SetBool(_moveAnimationKey, false);
        _animator.SetTrigger(_attackAnimationKey);
        _attackCrt = StartCoroutine(AttackCrt());
    }

    private IEnumerator AttackCrt()
    {
        _isStriking = true;
        yield return new WaitForSeconds(_animationHitDelay);
        _targetEnemy.TakeDamage(_attackDamage);
        yield return new WaitForSeconds(_endAttackCooldown);
        _isStriking = false;
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
                break;
            case PlayerState.Moving:
                if (_currentState == PlayerState.Attacking && _isStriking)
                {
                    CancelAttack();
                }
                _currentState = PlayerState.Moving;
                _animator.SetBool(_moveAnimationKey, true);
                break;
            case PlayerState.Attacking:
                if (_currentState == PlayerState.Idle)
                {
                    _animator.SetBool(_moveAnimationKey, false);
                }
                _currentState = PlayerState.Attacking;
                break;
        }
    }

    private void CheckTarget()
    {
        _targetEnemy = _enemiesManager.GetClosestAliveEnemyInConeRange(transform, _detectionAngle, _detectionRange);
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
        if (_hasTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_targetEnemy.transform.position, .5f);
        }
    }
    #endif
}
