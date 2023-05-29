using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
    private CinemachineBasicMultiChannelPerlin _camPerlin;

    public void Init ()
    {
        _camPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    
    public void DoShake(float amplitude, float intensity, float duration)
    {
        StartCoroutine(ProcessShake(amplitude, intensity, duration));
    }
    
    private IEnumerator ProcessShake(float amplitude, float intensity, float duration)
    {
        DoNoise(amplitude, intensity);
        yield return new WaitForSeconds(duration);
        DoNoise(0, 0);
    }
 
    private void DoNoise(float amplitudeGain, float frequencyGain)
    {
        _camPerlin.m_AmplitudeGain = amplitudeGain;
        _camPerlin.m_FrequencyGain = frequencyGain;
    }

}
