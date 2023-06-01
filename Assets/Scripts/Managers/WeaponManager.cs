using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour
{
    public Action<WeaponDataSo> OnWeaponChanged;
    public WeaponDataSo CurrentWeapon => _currentWeapon;

    [SerializeField] private WeaponDataSo[] _weaponsData;
    [SerializeField] private WeaponsWidgetUI _weaponsWidgetUI;

    private WeaponDataSo _currentWeapon;
    
    public void Init()
    {
        _weaponsWidgetUI.Init(_weaponsData);
        _weaponsWidgetUI.OnWeaponButtonSelected += SelectWeapon;
        SelectWeapon(_weaponsData[Random.Range(0, _weaponsData.Length)]);
    }

    private void SelectWeapon(WeaponDataSo weapon)
    {
        if (_currentWeapon == weapon) return;
        _weaponsWidgetUI.SetSelectedWeapon(weapon);
        _currentWeapon = weapon;
        OnWeaponChanged?.Invoke(weapon);
    }
}
