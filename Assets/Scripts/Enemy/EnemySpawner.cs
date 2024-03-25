using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float padding;
    [SerializeField] private int maxSpawn;
    [SerializeField] private LayerMask enemyLayer;
    private List<GameObject> activeEnemyList = new List<GameObject>();

    void OnEnable()
    {
        EnemyStats.onEnemyDie += RemoveEnemyFromList;
        DayNightManager.eventHitTheSack += RemoveAllEnemyFromList;
    }

    void OnDisable()
    {
        EnemyStats.onEnemyDie -= RemoveEnemyFromList;
        DayNightManager.eventHitTheSack -= RemoveAllEnemyFromList;
    }

    public void SpawnEnemy()
    {
        int count = 0;

        for(float i = -transform.localScale.x/2; i <= transform.localScale.x/2; i += UnityEngine.Random.Range(padding, padding + 3f))
        {
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x + i, transform.position.y), padding, enemyLayer) != null)
                continue;
            else
            {
                GameObject enemy = Instantiate(enemyToSpawn, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                count++;
                activeEnemyList.Add(enemy);
            }

            if(count >= maxSpawn)
                break;
        }
    }

    private void RemoveEnemyFromList(GameObject enemy) => activeEnemyList.Remove(enemy);
    private void RemoveAllEnemyFromList()
    {
        foreach(GameObject enemy in activeEnemyList)
            Destroy(enemy, 1);
        activeEnemyList.Clear();
    }
}
