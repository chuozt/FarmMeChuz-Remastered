using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightManager : Singleton<DayNightManager>
{
    [SerializeField] private Animator blackScreenAnimator;
    [SerializeField] private SpriteRenderer srDayNightBackGround;
    [SerializeField] private Gradient gradientDayNight;
    [SerializeField] private float cycleTime;
    [SerializeField] private Text dayNightText;
    [SerializeField] private Text timeInDayText;
    [SerializeField] private GameObject thinkingBubble;
    [SerializeField] private SpriteRenderer bedBorder;

    [SerializeField] private int currentDay = 1;
    float currentTime = 0;
    int currentHour, startHour = 6, totalHourInADay = 18;
    float minute;
    bool canSleep = false;
    bool enemyIsSpawned = false;
    bool isPlayerSleeping = false;
    bool isPlayerDying = false;
    bool isDay = true;

    public static event Action eventHitTheSack;
    public static event Action<float> eventHitTheSackFloat;

    void Start()
    {
        bedBorder.enabled = false;
        currentHour = startHour;
        isDay = true;
    }
    
    void LateUpdate()
    {
        UpdateTime();

        if(currentTime <= cycleTime * 0.55f)
        {
            isDay = true;
            dayNightText.text = "Day " + currentDay;
            enemyIsSpawned = false;
        }
        else
        {
            isDay = false;
            dayNightText.text = "Night " + currentDay;
        }

        if(!enemyIsSpawned && !isDay)
        {
            GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawners");
            foreach(var spawner in enemySpawners)
            {
                spawner.TryGetComponent<EnemySpawner>(out EnemySpawner es);
                es.SpawnEnemy();
            }
            enemyIsSpawned = true;
        }
        
        if(Player.Instance.IsInteracting)
            return;

        if(canSleep && Input.GetKeyDown(KeyCode.F) && isDay)
        {
            Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You can only sleep at night!"));
            CameraShake.Instance.ShakeCamera();
        }
        else if(canSleep && Input.GetKeyDown(KeyCode.F) && !isDay)
            StartCoroutine("HitTheSack");
    }

    void UpdateTime()
    {
        if(isPlayerSleeping || isPlayerDying)  
            return;

        if(currentTime <= cycleTime)
            currentTime += Time.deltaTime; 
        

        if(currentHour < 24)
            minute += (Time.deltaTime / cycleTime * totalHourInADay * 10);

        if(minute >= 10)
        {
            minute = 0;
            currentHour++;
        }

        if(currentHour < 22)
            thinkingBubble.SetActive(false);
        else
            thinkingBubble.SetActive(true);

        if(currentHour == 24)
            StartCoroutine("OnPlayerDieByNotSleep");

        timeInDayText.text = currentHour + ":" + (int)minute;
        
        srDayNightBackGround.color = gradientDayNight.Evaluate(currentTime/cycleTime);
        srDayNightBackGround.color = new Color(srDayNightBackGround.color.r, srDayNightBackGround.color.g, srDayNightBackGround.color.b, 0.5f * currentTime / cycleTime);
    }

    IEnumerator HitTheSack()
    {
        Player.Instance.SetInteractingState(false);
        isPlayerSleeping = true;

        blackScreenAnimator.Play("BlackScreen_FadeInAndOut");
        StartCoroutine(Player.Instance.DisableMovingAbilityTemporary(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length));
        yield return new WaitForSeconds(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length / 2);

        RefreshTimer();

        //Regen player's stats
        Player.Instance.SetCurrentHealth = Player.Instance.GetMaxHealth;
        Player.Instance.SetCurrentMana = Player.Instance.GetMaxMana;

        //Trigger all the events
        eventHitTheSack?.Invoke();
        eventHitTheSackFloat?.Invoke(cycleTime - currentTime);

        isPlayerSleeping = false;
        Player.Instance.SetInteractingState(true);
    }

    IEnumerator OnPlayerDieByNotSleep()
    {
        isPlayerDying = true;
        Player.Instance.SetDead();
        yield return new WaitForSeconds(Player.Instance.Anim.GetCurrentAnimatorStateInfo(0).length);

        blackScreenAnimator.Play("BlackScreen_FadeInAndOut");
        StartCoroutine(Player.Instance.DisableMovingAbilityTemporary(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length));
        yield return new WaitForSeconds(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length / 1.75f);

        Player.Instance.ResetToSpawnPoint();
        RefreshTimer();

        //Trigger all the events
        eventHitTheSack?.Invoke();
        eventHitTheSackFloat?.Invoke(cycleTime - currentTime);

        isPlayerDying = false;
    }

    public IEnumerator OnPlayerDieByHit()
    {
        isPlayerDying = true;
        yield return new WaitForSeconds(Player.Instance.Anim.GetCurrentAnimatorStateInfo(0).length);

        blackScreenAnimator.Play("BlackScreen_FadeInAndOut");
        StartCoroutine(Player.Instance.DisableMovingAbilityTemporary(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length));
        yield return new WaitForSeconds(blackScreenAnimator.GetCurrentAnimatorStateInfo(0).length / 1.75f);

        Player.Instance.ResetToSpawnPoint();
        isPlayerDying = false;
    }

    void RefreshTimer()
    {
        currentHour = startHour;
        minute = 0;
        currentDay++;
        currentTime = 0;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canSleep = true;
            bedBorder.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canSleep = false;
            bedBorder.enabled = false;
        }
    }

    public int CurrentDay { get{ return currentDay; } }
}
