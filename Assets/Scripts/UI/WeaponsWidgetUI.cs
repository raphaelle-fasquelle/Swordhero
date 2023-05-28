using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsWidgetUI : MonoBehaviour
{
    [SerializeField] private WeaponUI _weaponUI;

    public void Init(WeaponDataSo[] weapons, Action<WeaponDataSo> selectionCallback)
    {
        foreach (var weaponDataSo in weapons)
        {
            WeaponUI weaponUI = Instantiate(_weaponUI, transform);
            weaponUI.Setup(weaponDataSo, selectionCallback);
        }
    }
}
