using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonUI : MonoBehaviour
{
    public Action<WeaponDataSo> OnWeaponButtonSelected;
    
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _displayedName;
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    
    private WeaponDataSo _weaponData;
    
    public void Setup(WeaponDataSo weaponData)
    {
        _weaponData = weaponData;
        _button.onClick.AddListener(OnButtonClick);

        _icon.sprite = weaponData.Icon;
        _displayedName.text = weaponData.DisplayName;
    }

    public void SetSelected(WeaponDataSo weapon)
    {
        _button.enabled = weapon != _weaponData;
        _background.color = weapon == _weaponData ? Color.green : Color.white;
    }

    private void OnButtonClick()
    {
        OnWeaponButtonSelected.Invoke(_weaponData);
    }
}
