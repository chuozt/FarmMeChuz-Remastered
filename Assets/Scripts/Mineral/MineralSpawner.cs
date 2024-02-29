using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MineralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mineralPrefab;
    [field:SerializeField] List<Mineral> minerals;
    [SerializeField] private float padding;
    [SerializeField] private LayerMask mineralLayer;
    [SerializeField] private LayerMask groundLayer;

    void Start() => SpawnMineral();

    void OnEnable() => DayNightManager.eventHitTheSack += SpawnMineral;
    void OnDisable() => DayNightManager.eventHitTheSack -= SpawnMineral;

    [ContextMenu("SpawnMineral")]
    public void SpawnMineral()
    {
        bool isSpawnedSpecialMineral = false;

        for(float xScale = -transform.localScale.x/2; xScale <= transform.localScale.x/2; xScale += Random.Range(padding, padding + 0.45f))
        {
            float xPos = xScale + transform.position.x;
            float yPos = Random.Range(-transform.localScale.y/2, transform.localScale.y/2) + transform.position.y;
            
            RaycastHit2D rayCheck1 = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.up, 1, groundLayer);
            RaycastHit2D rayCheck2 = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.down, 1, groundLayer);

            if(rayCheck1 != rayCheck2)
            {
                float distance = transform.localScale.y/2 + yPos - transform.position.y;
                RaycastHit2D ray = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.down, distance, groundLayer);
            
                if(ray.collider != null)
                {
                    if(Physics2D.OverlapCircle(ray.point, 0.5f, mineralLayer) != null)
                        continue;
                    
                    int randomNum = GetRandomSpawn();
                    if(minerals[randomNum].mineralData.IsSpecialMineral)
                    {
                        //Special minerals can only be spawned each 3-day period
                        if(!(DayNightManager.Instance.CurrentDay % 3 == 0))
                            continue;
                        //Only 1 Special mineral can be spawned in a day
                        if(isSpawnedSpecialMineral)
                            continue;

                        isSpawnedSpecialMineral = true;
                    }

                    Vector3 pos = ray.point;
                    pos.y += 0.45f;
                    GameObject mineral = Instantiate(mineralPrefab, pos, Quaternion.identity);
                    mineral.GetComponent<MineralBehaviours>().mineralData = minerals[randomNum].mineralData;
                    mineral.GetComponent<MineralBehaviours>().UpdateMineral();

                    //Decrease an "Increased" amount
                    minerals[randomNum].percentages -= minerals[randomNum].mineralData.PercentageIncreasement;
                    //Decrease 5%
                    minerals[randomNum].percentages *= 0.95f;
                    
                }
            }
            else
                continue;
        }
    }
    
    public int GetRandomSpawn()
    {
        float random = Random.Range(0f, 1f);
        float numForAdding = 0;
        float total = 0;

        for(int i = 0; i < minerals.Count; i++)
        {
            total += minerals[i].percentages;
        }

        for(int i = 0; i < minerals.Count; i++)
        {
            if(minerals[i].percentages / total + numForAdding >= random)
                return i;
            else
                numForAdding += minerals[i].percentages/total;
        }

        return 0;
    }

    [ContextMenu("UpdateSpawnPercentages")]
    public void UpdateSpawnPercentages()
    {
        foreach(Mineral mineral in minerals)
            mineral.percentages += mineral.mineralData.PercentageIncreasement;
    }
}

[System.Serializable]
public class Mineral
{
    [SerializeField] public SO_Mineral mineralData;
    [SerializeField] public float percentages;
}
