using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public VirtualJoystick Joystick => _joystick;
    public PlayerController PlayerController => _playerController;
    public Camera Camera => _camera;

    [SerializeField] private VirtualJoystick _joystick;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _camera;

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
        Init();
    }

    private void Init()
    {
        _joystick.Init();
        _playerController.Init();
    }
}
