using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer srDayNightBackGround;
    [SerializeField] private Gradient gradientDayNight;
    [SerializeField] private float cycleTime;
    [SerializeField] private Text dayNightText;
    [SerializeField] private Text timeInDayText;

    
    [SerializeField] private GameObject thinkingBubble;
    [SerializeField] private SpriteRenderer bedBorder;

    private int currentDay = 1;
    private float currentTime = 0;
    [HideInInspector] public bool isDay = true;
    int hour, startHour = 6, totalHourInADay = 18;
    float minute;
    bool canSleep = false;
    bool enemyIsSpawned = false;

    //Managers
    Player player;
    Feedback feedback;
    CameraShake cameraShake;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        feedback = GameObject.FindGameObjectWithTag("FeedbackText").GetComponent<Feedback>();
        cameraShake = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CameraShake>();

        hour = startHour;
        isDay = true;
    }
    
    void Update()
    {
        if(currentTime <= cycleTime)
            currentTime += Time.deltaTime; 

        if(hour < 24)
            minute += (Time.deltaTime / cycleTime * totalHourInADay * 10);

        if(minute >= 10)
        {
            minute = 0;
            hour++;
        }

        if(hour < 22)
            thinkingBubble.SetActive(false);
        else
            thinkingBubble.SetActive(true);

        if(hour == 24)
        {
            player.TakeDamage(1000);
            HitTheSack();
        }

        timeInDayText.text = hour + ":" + (int)minute;
        
        srDayNightBackGround.color = gradientDayNight.Evaluate(currentTime/cycleTime);
        srDayNightBackGround.color = new Color(srDayNightBackGround.color.r, srDayNightBackGround.color.g, srDayNightBackGround.color.b, 0.5f * currentTime / cycleTime);

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

        if(canSleep && Input.GetKeyDown(KeyCode.F) && isDay)
        {
            feedback.StartCoroutine(feedback.FeedbackTrigger("You can only sleep at night!"));
            cameraShake.ShakeCamera();
        }
        else if(canSleep && Input.GetKeyDown(KeyCode.F) && !isDay)
        {
            player.SetCurrentHealth = 100;
            player.SetCurrentMana = 100;
            HitTheSack();
        } 
    }

    void HitTheSack()
    {
        hour = startHour;
        minute = 0;

        GameObject[] growingCrops = GameObject.FindGameObjectsWithTag("GrowingCrops");
        foreach(var crop in growingCrops)
        {
            crop.TryGetComponent<GrowingCrop_ParentClass>(out GrowingCrop_ParentClass growingCrop);

            if(growingCrop != null)
                growingCrop.DecreaseTimeAfterDay(cycleTime - currentTime);
        }

        GameObject[] mineralSpawners = GameObject.FindGameObjectsWithTag("MineralSpawners");
        foreach(var spawner in mineralSpawners)
        {
            spawner.TryGetComponent<MineralSpawner>(out MineralSpawner ms);
            ms.SpawnMineral();
        }

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach(var tree in trees)
        {
            tree.TryGetComponent<TreeBehaviours>(out TreeBehaviours tb);
            tb.TreeGrowUp();
        }

        GameObject[] treeSpawners = GameObject.FindGameObjectsWithTag("TreeSpawners");
        foreach(var spawner in treeSpawners)
        {
            spawner.TryGetComponent<TreeSpawner>(out TreeSpawner ts);
            ts.SpawnTree();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies)
        {
            Destroy(enemy);
        }

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
}
