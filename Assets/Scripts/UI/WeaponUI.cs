using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _displayedName;
    [SerializeField] private Button _button;
    
    private WeaponDataSo _weaponData;
    
    public void Setup(WeaponDataSo weaponData, Action<WeaponDataSo> selectionCallback)
    {
        _weaponData = weaponData;
        _button.onClick.AddListener(()=>selectionCallback(_weaponData));

        _icon.sprite = weaponData.Icon;
        _displayedName.text = weaponData.DisplayName;
    }
}
