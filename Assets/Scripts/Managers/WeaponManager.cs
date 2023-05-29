using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
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
        if (_currentWeapon != weapon)
        {
            _weaponsWidgetUI.SetSelectedWeapon(weapon);
            GameManager.Instance.PlayerController.PlayerWeapon.SetWeapon(weapon);
            _currentWeapon = weapon;
        }
    }
}
