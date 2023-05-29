using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponsWidgetUI : MonoBehaviour
{
    public Action<WeaponDataSo> OnWeaponButtonSelected;
    
    [SerializeField] private WeaponButtonUI weaponButtonUI;

    private WeaponButtonUI[] _weaponButtons;
    
    public void Init(WeaponDataSo[] weapons)
    {
        _weaponButtons = new WeaponButtonUI[weapons.Length];
        for (var index = 0; index < weapons.Length; index++)
        {
            _weaponButtons[index] = Instantiate(weaponButtonUI, transform);
            _weaponButtons[index].Setup(weapons[index]);
            _weaponButtons[index].OnWeaponButtonSelected += OnWeaponButtonClicked;
        }
    }

    public void SetSelectedWeapon(WeaponDataSo weapon)
    {
        foreach (var weaponButton in _weaponButtons)
        {
            weaponButton.SetSelected(weapon);
        }
    }

    private void OnWeaponButtonClicked(WeaponDataSo weaponDataSo)
    {
        OnWeaponButtonSelected.Invoke(weaponDataSo);
    }
}
