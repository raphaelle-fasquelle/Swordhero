using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public VirtualJoystick Joystick => _joystick;
    public PlayerController PlayerController => _playerController;
    public Camera Camera => _camera;
    public EnemiesManager EnemiesManager => _enemiesManager;
    public TargetIndicator TargetIndicator => _targetIndicator;
    public WeaponManager WeaponManager => _weaponManager;

    [SerializeField] private VirtualJoystick _joystick;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _camera;
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private TargetIndicator _targetIndicator;
    [SerializeField] private WeaponManager _weaponManager;

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Init();
    }

    private void Init()
    {
        _joystick.Init();
        _weaponManager.Init();
        _enemiesManager.Init();
        _playerController.Init();
    }
}
