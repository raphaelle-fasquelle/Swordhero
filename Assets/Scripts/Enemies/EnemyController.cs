using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    public bool IsAlive => _currentLifePoints > 0;
    
    [SerializeField] private int _lifePoints;
    [SerializeField] private Animator _animator;

    private int _currentLifePoints;
    
    //ANIMATION KEYS
    private int _deathAnimationKey = Animator.StringToHash("Die");
    private int _damageAnimationKey = Animator.StringToHash("TakeDamage");

    public void Init()
    {
        _currentLifePoints = _lifePoints;
    }

    public float SqrDistanceToPosition(Vector3 position)
    {
        return Vector3.SqrMagnitude(position - transform.position);
    }

    public bool IsInCone(Vector3 position, Vector3 coneDirection, float coneAngle)
    {
        return Vector3.Angle(coneDirection, transform.position - position) <= coneAngle / 2f;
    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
        _animator.SetTrigger(_damageAnimationKey);
        if (_currentLifePoints <= 0)
        {
            _currentLifePoints = 0;
            _animator.SetTrigger(_deathAnimationKey);
        }
    }
}
