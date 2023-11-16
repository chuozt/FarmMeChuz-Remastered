using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float intensity = 0.75f;
    [SerializeField] private float duration;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    [SerializeField] private AudioClip sfxError;

    void Start()
    {
        StopShake();
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();    
    }

    public void ShakeCamera()
    {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = intensity;
        AudioSource.PlayClipAtPoint(sfxError, this.transform.position);

        timer = duration;
    }

    void StopShake()
    {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0;
        timer = 0;
    }
    
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }
    }
}
