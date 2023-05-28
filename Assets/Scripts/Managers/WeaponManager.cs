using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponDataSo[] _weaponsData;

    public void Init()
    {
        GameManager.Instance.PlayerController.PlayerWeapon.SetWeapon(_weaponsData[Random.Range(0, _weaponsData.Length)]);
    }
}
