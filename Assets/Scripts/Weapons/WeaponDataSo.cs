using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon Data", order = 1)]
public class WeaponDataSo : ScriptableObject
{
    public string ID => _id;

    public float TimingToHitEffect => _timingToHitEffect;

    public float EndAttackCooldown => _endAttackCooldown;

    public float AttackSpeedMultiplier => _attackSpeedMultiplier;

    public float MovementSpeedMultiplier => _movementSpeedMultiplier;
    
    public float AttackRange => _attackRange;

    public int Damage => _damage;

    public GameObject WeaponObject => _weaponObject;

    public string DisplayName => _displayName;

    public Sprite Icon => _icon;

    public float ShakeAmplitude => _shakeAmplitude;

    public float ShakeIntensity => _shakeIntensity;

    public float ShakeDuration => _shakeDuration;

    public ParticleSystem HitFx => _hitFx;

    [Header("Setup")]
    [SerializeField] private string _id; //COULD HAVE BEEN USED TO SAVE LAST WEAPON USED AND START WITH IT INSTEAD OF RANDOM WEAPON
    [Space(10)]
    [Header("Tweakables Gameplay")]
    [SerializeField] private float _attackSpeedMultiplier;
    [SerializeField] private float _movementSpeedMultiplier;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [Header("Tweakables Feedbacks")]
    [SerializeField] private float _timingToHitEffect;
    [SerializeField] private float _endAttackCooldown;
    [Space(10)]
    [Header("Feedback Parameters")]
    [SerializeField] private float _shakeAmplitude;
    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _shakeDuration;
    [Space(10)]
    [Header("Links")]
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ParticleSystem _hitFx;
}
