using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.forward = _cam.transform.forward;
    }
}
