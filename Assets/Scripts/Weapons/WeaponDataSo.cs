using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon Data", order = 1)]
public class WeaponDataSo : ScriptableObject
{
    public string ID => _id;

    public float TimingToHitEffect => _timingToHitEffect;

    public float EndAttackCooldown => _endAttackCooldown;

    public float MovementSpeedMultiplier => _movementSpeedMultiplier;
    
    public float AttackRange => _attackRange;

    public GameObject WeaponObject => _weaponObject;

    public string DisplayName => _displayName;

    public Sprite Icon => _icon;

    [Header("Setup")]
    [SerializeField] private string _id;
    [Space(10)]
    [Header("Tweakables")]
    [SerializeField] private float _timingToHitEffect;
    [SerializeField] private float _endAttackCooldown;
    [SerializeField] private float _movementSpeedMultiplier;
    [SerializeField] private float _attackRange;
    [Space(10)]
    [Header("Links")]
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
}
