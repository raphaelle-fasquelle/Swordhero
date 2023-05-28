using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon Data", order = 1)]
public class WeaponDataSo : ScriptableObject
{
    public string ID => _id;

    public float TimingToHitEffect => _timingToHitEffect;

    public float MovementSpeed => _movementSpeed;

    public float AttackRange => _attackRange;

    public GameObject WeaponObject => _weaponObject;

    public string DisplayName => _displayName;

    public Sprite Icon => _icon;

    [SerializeField] private string _id;
    [SerializeField] private float _timingToHitEffect;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _attackMovementSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
}
