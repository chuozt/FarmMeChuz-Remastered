using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCrop_ParentClass : MonoBehaviour
{
    [SerializeField] protected ItemCropSeed cropSeedData;
    [SerializeField] protected List<Renderer> stagesList = new List<Renderer>();
    [SerializeField] protected GameObject waterBubblePrefab;
    [SerializeField] protected List<AudioClip> sfxList;

    protected GameObject waterBubble;
    [SerializeField] protected bool isWatered = false;
    protected bool canBeHavested = false;
    protected bool isInHarvestZone = false;
    protected float timeLeft;
    protected int progressIndex = 0;

    void OnEnable() => DayNightManager.eventHitTheSackFloat += DecreaseTimeAfterDay;
    void OnDisable() => DayNightManager.eventHitTheSackFloat -= DecreaseTimeAfterDay;

    void Awake()
    {
        timeLeft = cropSeedData.TimeToGrow;
        if(!isWatered)
        {   
            waterBubble = Instantiate(waterBubblePrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            waterBubble.transform.SetParent(transform);
        }
        SetStage(progressIndex);
    }

    void Update()
    {
        if(!isWatered)
            return;

        if(timeLeft >= 0)
            timeLeft -= Time.deltaTime;
        
        if((timeLeft <= cropSeedData.TimeToGrow/2 && timeLeft > 0 && progressIndex == 1) || timeLeft <= 0)
            SetStage(progressIndex);

        if(canBeHavested && Input.GetMouseButtonDown(1) && isInHarvestZone)
            Havest();
    }

    protected void SetStage(int index)
    {
        //Disable all the renderers
        foreach(Renderer stage in stagesList)
            stage.enabled = false;

        //Enable the parameter renderer
        stagesList[index].enabled = true;
        if(progressIndex < 2)
            progressIndex++;
        else if(progressIndex == 2)
            canBeHavested = true;
    }

    protected virtual void Havest()
    {
        stagesList[2].enabled = false;
        
        //Create random spawn
        int randomNum = Random.Range(cropSeedData.MinSpawningCrops,cropSeedData.MaxSpawningCrops + 1);

        //Spawn the crops
        for(int i = 0; i < randomNum; i++)
        {
            GameObject crop = Instantiate(cropSeedData.DropCrop, transform.position, Quaternion.Euler(0,0, Random.Range(-15f, 15f)));
            crop.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.3f, 0.3f), 0.3f) * 1.3f, ForceMode2D.Impulse);
            Destroy(crop, 300);
        }
        
        AudioManager.Instance.PlaySFX(sfxList[Random.Range(0, sfxList.Count)]);
    }

    public void DecreaseTimeAfterDay(float time)
    {
        timeLeft -= time;
    }

    public void WaterTheCrop()
    {
        isWatered = true;
        waterBubble.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
            isInHarvestZone = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
            isInHarvestZone = false;
    }
}
