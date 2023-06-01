using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform _weapondHolder;

    private GameObject _spawnedWeapon;

    public void Init()
    {
        SetWeapon(GameManager.Instance.WeaponManager.CurrentWeapon);
        GameManager.Instance.WeaponManager.OnWeaponChanged += SetWeapon;
    }
    
    private void SetWeapon(WeaponDataSo weaponDataSo)
    {
        Debug.Log("SET PLAYER WEAPON TO "+weaponDataSo.DisplayName);
        if (_spawnedWeapon != null)
        {
            Destroy(_spawnedWeapon);
        }

        _spawnedWeapon = Instantiate(weaponDataSo.WeaponObject, _weapondHolder);
    }
}
