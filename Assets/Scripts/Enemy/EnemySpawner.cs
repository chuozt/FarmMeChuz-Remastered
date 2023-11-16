using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float padding;
    [SerializeField] private int maxSpawn;
    [SerializeField] private LayerMask enemyLayer;  

    public void SpawnEnemy()
    {
        int count = 0;

        for(float i = -transform.localScale.x/2; i <= transform.localScale.x/2; i += Random.Range(padding, padding + 3f))
        {
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x + i, transform.position.y), padding, enemyLayer) != null)
            {
                continue;
            }
            else
            {
                GameObject obj = Instantiate(enemyToSpawn, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                count++;
            }

            if(count >= maxSpawn)
            {
                break;
            }
        }
    }
}
