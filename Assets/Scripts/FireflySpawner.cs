using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireflyGameObject;
    [SerializeField] float paddingX, paddingY;

    int count = 0;
    List<GameObject> activeFireflyGameObjectList = new List<GameObject>();
    
    void OnEnable()
    {
        DayNightManager.eventHitTheSack += RefreshFirefly;
        DayNightManager.eventOnNight += SpawnFirefly;
    }
    void OnDisable()
    {
        DayNightManager.eventHitTheSack -= RefreshFirefly;
        DayNightManager.eventOnNight -= SpawnFirefly;
    }

    [ContextMenu("SpawnFirefly")]
    void SpawnFirefly()
    {
        float randomInitialXPos = Random.Range(1, 4);
        for(float i = -transform.localScale.x/2 + randomInitialXPos; i <= transform.localScale.x/2 + randomInitialXPos; i += Random.Range(paddingX, paddingX + 2f))
        {
            int randomNumber = Random.Range(0, 4);
            if(randomNumber != 0)
                continue;
            for(float j = -transform.localScale.y/2; j <= transform.localScale.y/2; j += Random.Range(paddingY, paddingY + 2))
            {
                randomNumber = Random.Range(0, 4);
                if(randomNumber != 0)
                    continue;

                Vector2 spawnPos = new Vector2(transform.position.x + i, transform.position.y + j);
                count++;

                if(activeFireflyGameObjectList.Count < count)
                {
                    GameObject firefly = Instantiate(fireflyGameObject, this.transform);
                    activeFireflyGameObjectList.Add(firefly);
                    firefly.transform.position = spawnPos;
                }
                else
                {
                    for(int k = 0; k < activeFireflyGameObjectList.Count; k++)
                    {
                        if(activeFireflyGameObjectList[k].activeInHierarchy)
                            continue;
                        
                        activeFireflyGameObjectList[k].SetActive(true);
                        activeFireflyGameObjectList[k].GetComponent<ParticleSystem>().Play();
                        activeFireflyGameObjectList[k].transform.position = spawnPos;
                        break;
                    }
                }

                break;
            }
        }
    }

    [ContextMenu("RefreshFirefly")]
    void RefreshFirefly()
    {
        count = 0;
        for(int i = 0; i < activeFireflyGameObjectList.Count; i++)
            activeFireflyGameObjectList[i].SetActive(false);
    }
}
