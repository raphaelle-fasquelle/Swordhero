using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    public bool IsAlive => _currentLifePoints > 0;
    
    //PARAMETERS
    [SerializeField] private int _lifePoints;
    //FEEDBACKS
    [SerializeField] private float _damageBlinkDuration = .15f;
    //LINKS
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private LifeGauge _lifeGauge;

    private EnemiesManager _enemiesManager;
    
    private int _currentLifePoints;

    private Material _defaultMat;

    private Sequence _blinkSeq;
    
    //ANIMATION KEYS
    private int _deathAnimationKey = Animator.StringToHash("Die");
    private int _damageAnimationKey = Animator.StringToHash("TakeDamage");

    public void Init(EnemiesManager enemiesManager)
    {
        _enemiesManager = enemiesManager;
        _defaultMat = _renderer.sharedMaterial;
    }

    public void Spawn(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.localScale = Vector3.one;
        _currentLifePoints = _lifePoints;
        _lifeGauge.ResetGauge();
    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
        Blink();
        _animator.SetTrigger(_damageAnimationKey);
        if (_currentLifePoints <= 0)
        {
            _currentLifePoints = 0;
            Die();
        }
        _lifeGauge.UpdateGauge((float)_currentLifePoints/_lifePoints);
    }

    public float SqrDistanceToPosition(Vector3 position)
    {
        return Vector3.SqrMagnitude(position - transform.position);
    }

    public bool IsInCone(Vector3 position, Vector3 coneDirection, float coneAngle)
    {
        return Vector3.Angle(coneDirection, transform.position - position) <= coneAngle / 2f;
    }

    private void Blink()
    {
        if (_blinkSeq != null)
        {
            _blinkSeq.Kill(true);
        }
        _renderer.material = _blinkMaterial;
        _blinkSeq = DOTween.Sequence();
        _blinkSeq.AppendInterval(_damageBlinkDuration);
        _blinkSeq.OnComplete(() =>
        {
            _renderer.material = _defaultMat;
            _blinkSeq = null;
        });
    }

    private void Die()
    {
        _animator.SetTrigger(_deathAnimationKey);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.5f);
        sequence.Append(transform.DOScale(0, .3f).SetEase(Ease.InBack));
        sequence.AppendCallback(() =>
        {
            _enemiesManager.ReturnToPool(this);
        });
    }
}
