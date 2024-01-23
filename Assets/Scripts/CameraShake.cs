using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
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

    public void ShakeCamera()
    {
        _cbmcp.AmplitudeGain = intensity;
        AudioSource.PlayClipAtPoint(sfxError, this.transform.position);

        timer = duration;
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
