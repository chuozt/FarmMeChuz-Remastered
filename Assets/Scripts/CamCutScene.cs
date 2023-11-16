using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamCutScene : MonoBehaviour
{
    public PostProcessVolume ppVolume;
    private Bloom bloom;
    private Vignette vignette;
    public float timeToVignette;
    public float timeToBloom;
    public float timeToStopPPVolume;
    public float timeToPopUpESC;
    float time = 0;
    bool canExit = false;

    public Button a,b,c,d,e;
    public Text endText; 

    void Awake()
    {
        endText.enabled = false;
        a.enabled = false;
        b.enabled = false;
        c.enabled = false;
        d.enabled = false;
    }

    void Update()
    {
        time = Mathf.MoveTowards(time, 100, Time.deltaTime);

        //start vignette
        if(time > timeToVignette && time < timeToStopPPVolume)
        {
            ppVolume.profile.TryGetSettings(out vignette);
            vignette.intensity.Interp(vignette.intensity, 0.33f, 1f * Time.deltaTime);
        }

        //stop vignette, start bloom
        if(time > timeToBloom && time < timeToStopPPVolume)
        {
            ppVolume.profile.TryGetSettings(out bloom);
            bloom.intensity.Interp(bloom.intensity, 62f, 0.15f * Time.deltaTime);
            vignette.intensity.Interp(vignette.intensity, 0.05f, 1f * Time.deltaTime);
        }

        //stop ppVolume
        if(time >= timeToStopPPVolume)
        {
            ppVolume.enabled = false;
            a.enabled = true;
            b.enabled = true;
            c.enabled = true;
            d.enabled = true;
        }

        //pop up "ESC key to..."
        if(time >= timeToPopUpESC)
        {
            endText.enabled = true;
            canExit = true;   
        }
        if(Input.GetKeyDown(KeyCode.Escape) && canExit)
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }
}
