using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> treesToSpawn;
    [SerializeField] private float padding;
    [SerializeField] private LayerMask treeZoneLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private LayerMask layersTreeAffect;
    [SerializeField] private List<float> percentages;

    void Start()
    {
        SpawnTree();
    }

    public void SpawnTree()
    {
        for(float xScale = -transform.localScale.x/2; xScale <= transform.localScale.x/2; xScale += Random.Range(padding, padding + 0.5f))
        {
            float xPos = xScale + transform.position.x;
            float yPos = Random.Range(-transform.localScale.y/2, transform.localScale.y/2) + transform.position.y;
            float distance = transform.localScale.y/2 + yPos - transform.position.y;

            RaycastHit2D rayCheck1 = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.up, 3, grassLayer);
            RaycastHit2D rayCheck2 = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.down, distance, grassLayer);

            if(rayCheck1 != rayCheck2)
            {
                RaycastHit2D ray = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.down, distance, grassLayer);
            
                if(ray.collider != null)
                {
                    if(Physics2D.OverlapCircle(ray.point, 0.5f, treeZoneLayer) == null)
                    {
                        GameObject tree = Instantiate(treesToSpawn[GetRandomSpawn()], ray.point, Quaternion.identity);
                        tree.transform.position -= new Vector3(0, 0.02f, 0);
                        GameObject treeZone = tree.transform.GetChild(0).gameObject;

                        Collider2D col1 = Physics2D.OverlapBox(new Vector2(treeZone.transform.position.x, treeZone.transform.position.y + 0.1f), treeZone.transform.localScale, 0, groundLayer);
                        if(col1 != null)
                        {
                            Destroy(tree);
                        }

                        Collider2D[] col2 = Physics2D.OverlapBoxAll(new Vector2(treeZone.transform.position.x, treeZone.transform.position.y + 0.1f), treeZone.transform.localScale, 0, layersTreeAffect);
                        if(col2.Length > 2)
                        {
                            Destroy(tree);
                        }
                    }
                }
            }
            else
            {
                continue;
            }
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

        for(int i = 0; i < treesToSpawn.Count; i++)
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
