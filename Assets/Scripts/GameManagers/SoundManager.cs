using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public RainingSystem rainingSystem;
    public AudioSource sfxRain, sfxThunder;
    private bool isPlayingRain = false, isPlayingThunder = false;

    [Space(20)]
    public List<AudioClip> bgm;
    float t = 0;
    float t2 = 0;
    int randomNum;

    void Start() => randomNum = Random.Range(400, 500);

    // Update is called once per frame
    void Update()
    {
        HandleRaining();   
        HandleBackgroundMusic();
    }

    void HandleRaining()
    {
        //if isRaining is triggered
        if(rainingSystem.isRaining)
        {
            //play te thunder sfx if it have not been played yet
            if(!isPlayingThunder)
            {
                sfxThunder.Play();  
                isPlayingThunder = true;
            }

            t = Mathf.MoveTowards(t, 6, Time.deltaTime);
            //after an amount of time "t", play raining sfx after thunder sfx
            if(t > 5 && !isPlayingRain)
            {
                sfxRain.Play();
                isPlayingRain = true;
            }
        }
        else
        {
            //reset
            t = 0;
            isPlayingRain = false;
            isPlayingThunder = false;
        }        
    }

    void HandleBackgroundMusic()
    {
        t2 = Mathf.MoveTowards(t2, randomNum + 10, Time.deltaTime);
        //play the background music randomly
        if(t2 > randomNum)
        {
            AudioClip music = bgm[Random.Range(0, bgm.Count)];
            AudioManager.Instance.PlayBackgroundMusic(music);

            randomNum = Random.Range(250, 350) + (int)music.length;
            Debug.Log(randomNum + "    " + (int)music.length);
            t2 = 0;
        }
    }
}
