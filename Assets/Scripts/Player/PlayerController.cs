using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [Header("Movement")]
    [SerializeField] private  float _rotationSpeed;
    [SerializeField, Range(0f,1f)] private  float _minimumMoveInput = .3f;
    [SerializeField] private float _baseSpeed;
    //ATACK
    [Space(10)]
    [Header("Attack")]
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _detectionAngle;
    [SerializeField, Range(0f,1f)] private float _critChance;
    //LINKS
    [Space(10)]
    [Header("Links")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private FlyingText3d _damageFeedback;
    
    //EXTERNAL LINKS
    private VirtualJoystick _joystick;
    private EnemiesManager _enemiesManager;
    private TargetIndicator _targetIndicator;
    private CameraManager _cameraManager;
    private WeaponDataSo _currentWeapon => GameManager.Instance.WeaponManager.CurrentWeapon;
    
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
        _joystick = GameManager.Instance.Joystick;
        _enemiesManager = GameManager.Instance.EnemiesManager;
        _targetIndicator = GameManager.Instance.TargetIndicator;
        _cameraManager = GameManager.Instance.CameraManager;
        _movementCameraMultiplier = GameManager.Instance.Camera.transform.forward.z > 0 ? 1 : -1;

        _playerWeapon.Init();

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
        if (!_targetEnemy.IsAlive)
        {
            _targetIndicator.Disable();
            _targetEnemy = null;
            UpdateState(PlayerState.Idle);
            return;
        }
        
        if(_isStriking) return;
        
        Vector3 direction = (_targetEnemy.transform.position - transform.position).normalized;
        transform.forward = Vector3.RotateTowards(transform.forward,direction, _rotationSpeed * Time.deltaTime, 1);
        if (IsInRange(_targetEnemy.transform.position, _currentWeapon.AttackRange))
        {
            DealAttack();
        }
        else //NOT USED RIGHT NOW, USED IN A PREVIOUS VERSION WHERE THERE WAS A DETECTION RANGE IN ADDITION TO THE ATTACK RANGE
        {
            _animator.SetBool(_moveAnimationKey, true);
            transform.position = Vector3.MoveTowards(transform.position
                , _targetEnemy.transform.position - 1f * direction,
                Time.deltaTime * _baseSpeed * _currentWeapon.MovementSpeedMultiplier);
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
        bool isCriticalHit = Random.Range(0f, 1f) < _critChance;
        int damage = _currentWeapon.Damage * (isCriticalHit ? 2 : 1);
        
        _targetEnemy.TakeDamage(damage);
        DoHitFeedbacks(damage, isCriticalHit);
    }

    private void DoHitFeedbacks(int damage, bool isCriticalHit)
    {
        FlyingText3d damageFeedback = (FlyingText3d)GameManager.Instance.PoolManager.GetPool(_damageFeedback).Pick();
        damageFeedback.DoFly("-" + damage, .5f * Random.insideUnitSphere + _targetEnemy.transform.position
            , isCriticalHit ? Color.red : Color.white
            , () => GameManager.Instance.PoolManager.GetPool(_damageFeedback).ReturnToPool(damageFeedback));
        
        _cameraManager.DoShake(_currentWeapon.ShakeAmplitude,_currentWeapon.ShakeIntensity,_currentWeapon.ShakeDuration);
        
        ParticleSystem fx = (ParticleSystem)GameManager.Instance.PoolManager.GetPool(_currentWeapon.HitFx).Pick();
        fx.transform.position = _targetEnemy.transform.position;
        fx.Play();
        Sequence fxSeq = DOTween.Sequence();
        fxSeq.AppendInterval(fx.main.duration);
        fxSeq.AppendCallback(() =>
        {
            GameManager.Instance.PoolManager.GetPool(_currentWeapon.HitFx).ReturnToPool(fx);
        });
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
