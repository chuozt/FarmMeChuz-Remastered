using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : Singleton<CameraShake>
{
    private CinemachineCamera cinemachineCamera;
    [SerializeField] private float intensity = 0.75f;
    [SerializeField] private float duration = 0.5f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    [SerializeField] private AudioClip sfxError;

    void Start()
    {
        StopShake();
    }

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();   
        _cbmcp = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>(); 
    }

    public void ShakeCameraDamagingPlayer(AudioClip audioClip)
    {
        _cbmcp.AmplitudeGain = intensity;
        timer = duration;
        AudioManager.Instance.PlaySFX(audioClip);
    }

    public void ShakeCamera()
    {
        _cbmcp.AmplitudeGain = intensity;
        timer = duration;
        AudioManager.Instance.PlaySFX(sfxError);
    }

    public void ShakeCamera(float intensity, float duration)
    {
        _cbmcp.AmplitudeGain = intensity;
        timer = duration;
        AudioManager.Instance.PlaySFX(sfxError);
    }

    void StopShake()
    {
        _cbmcp.AmplitudeGain = 0;
        timer = 0;
    }
    
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
                StopShake();
        }
    }
}
