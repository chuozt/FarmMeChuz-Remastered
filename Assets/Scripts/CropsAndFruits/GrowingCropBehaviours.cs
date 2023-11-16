using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCropBehaviours : MonoBehaviour
{
    [Space(10)]
    public ItemCropSeed cropSeedData;

    [Space(10)]
    public Renderer stage1;
    public Renderer stage2;
    public Renderer stage3;

    [Space(10)]
    public GameObject cropToSpawn;
    [SerializeField] private GameObject needWaterBubble;
    GameObject waterBubble;
    bool canBeHavested = false;
    bool isInZone = false;
    [SerializeField] bool isWatered = false;
    float timeToGrow;
    float timeLeft;
    float timeBetweenStages;

    [Space(10)]
    public List<AudioClip> sfx;

    void Awake()
    {
        timeToGrow = cropSeedData.TimeToGrow - (cropSeedData.TimeToGrow * cropSeedData.BuffPercentage/100);
        timeLeft = timeToGrow;
        timeBetweenStages = timeToGrow/2;
        waterBubble = Instantiate(needWaterBubble, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        waterBubble.transform.SetParent(transform);
        SetStage(stage1);
    }

    void Update()
    {
        if(isWatered)
        {
            waterBubble.SetActive(false);
            
            if(timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime;
            }
            
            if(timeLeft <= timeBetweenStages && timeLeft > 0)
            {
                SetStage(stage2);
            }
            else if(timeLeft <= 0)
            {
                SetStage(stage3);
                canBeHavested = true;
            }

            if(canBeHavested && Input.GetMouseButtonDown(1) && isInZone)
            {
                Havest();
            }
        }
    }

    void SetStage(Renderer currentStage)
    {
        //Disable all the renderers
        stage1.enabled = false;
        stage2.enabled = false;
        stage3.enabled = false;

        //Enable the parameter renderer
        currentStage.enabled = true;
    }

    void Havest()
    {
        stage3.enabled = false;
        int randomNum = Random.Range(cropSeedData.MinSpawningCrops,cropSeedData.MaxSpawningCrops + 1);
        for(int i = 0; i < randomNum; i++)
        {
            GameObject crop = Instantiate(cropToSpawn, this.transform.position, Quaternion.Euler(0,0, Random.Range(-15f, 15f)));
            crop.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.3f, 0.3f), 0.3f) * 1.3f, ForceMode2D.Impulse);
            Destroy(crop, 300);
        }
        AudioSource.PlayClipAtPoint(sfx[Random.Range(0, sfx.Count)], transform.position);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            isInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            isInZone = false;
        }
    }

    public void DecreaseTime(float time)
    {
        timeLeft -= time;
    }

    public bool IsWatered 
    {
        set 
        {
            isWatered = value;
        }
    }
}
