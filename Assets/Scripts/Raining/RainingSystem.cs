using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainingSystem : MonoBehaviour
{
    public GameObject rainingBG;
    private SpriteRenderer sr;

    public Color clearCloud;
    public Color rainyCloud;

    private float _timeUntilRain = 0;
    public float timeBetweenRains = 200;

    private float currentRainingTime = 0;
    public float rainingTime = 20;

    public bool isRaining = false;
    float randomTimeUntilRain;
    
    void Start()
    {
        sr = rainingBG.GetComponent<SpriteRenderer>();
        sr.color = clearCloud;
        randomTimeUntilRain = Random.Range(timeBetweenRains - 30, timeBetweenRains + 30);
    }

    void Update()
    {
        if(_timeUntilRain < randomTimeUntilRain)
        {
            _timeUntilRain = Mathf.MoveTowards(_timeUntilRain, randomTimeUntilRain, Time.deltaTime);
            OffRainingWeather();
        }
        else if(_timeUntilRain > randomTimeUntilRain - 1)
        {
            OnRainingWeather();
            if(currentRainingTime > rainingTime - 1)
            {
                randomTimeUntilRain = Random.Range(timeBetweenRains - 50, timeBetweenRains + 50);
                _timeUntilRain = 0;
                currentRainingTime = 0;
            }
        }
    }

    void OnRainingWeather()
    {
        isRaining = true;
        sr.color = Color.Lerp(sr.color, rainyCloud, 0.2f * Time.deltaTime);
        currentRainingTime = Mathf.MoveTowards(currentRainingTime, rainingTime, Time.deltaTime);
    }

    void OffRainingWeather()
    {
        isRaining = false;
        sr.color = Color.Lerp(sr.color, clearCloud, 0.2f * Time.deltaTime);
    }
}
