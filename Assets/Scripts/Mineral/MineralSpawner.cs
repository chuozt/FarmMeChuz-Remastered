using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mineralPrefab;
    [SerializeField] private List<SO_Mineral> mineralDatas;
    [SerializeField] private float padding;
    [SerializeField] private LayerMask mineralLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private List<float> percentages;

    void Start()
    {
        SpawnMineral();
    }

    public void SpawnMineral()
    {
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
                    if(Physics2D.OverlapCircle(ray.point, 0.5f, mineralLayer) == null)
                    {
                        Vector3 pos = ray.point;
                        pos.y += 0.45f;
                        GameObject mineral = Instantiate(mineralPrefab, pos, Quaternion.identity);
                        mineral.GetComponent<MineralBehaviours>().mineralData = mineralDatas[GetRandomSpawn()];
                        mineral.GetComponent<MineralBehaviours>().UpdateMineral();
                    }
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

        for(int i = 0; i < percentages.Count; i++)
        {
            total += percentages[i];
        }

        for(int i = 0; i < mineralDatas.Count; i++)
        {
            if(percentages[i] / total + numForAdding >= random)
            {
                return i;
            }
            else
            {
                numForAdding += percentages[i]/total;
            }
        }

        return 0;
    }
}
