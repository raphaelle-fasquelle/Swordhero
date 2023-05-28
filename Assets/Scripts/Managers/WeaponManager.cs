using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponDataSo[] _weaponsData;
    [SerializeField] private WeaponsWidgetUI _weaponsWidgetUI;

    public void Init()
    {
        SwapWeapon(_weaponsData[Random.Range(0, _weaponsData.Length)]);
        _weaponsWidgetUI.Init(_weaponsData, SwapWeapon);
    }

    public void SwapWeapon(WeaponDataSo weapon)
    {
        GameManager.Instance.PlayerController.PlayerWeapon.SetWeapon(weapon);
    }
}
