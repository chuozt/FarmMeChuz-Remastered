using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStats : MonoBehaviour
{
    public Enemy enemyData;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackForce;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isTakingDamage = false;
    [SerializeField] private List<AudioClip> sfxEnemyHurtList;
    [SerializeField] private AudioClip sfxEnemyDead;

    public static event Action<GameObject> onEnemyDie;

    float time = 0;
    private float health;
    private float damage;

    void Awake()
    {
        health = enemyData.Health;
        damage = enemyData.Damage;
    }

    void Update()
    {
        if(isTakingDamage)
        {
            time = Mathf.MoveTowards(time, anim.GetCurrentAnimatorStateInfo(0).length, Time.deltaTime);
            if(time >= anim.GetCurrentAnimatorStateInfo(0).length)
            {
                isTakingDamage = false;
                time = 0;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health > 0)
        {
            anim.SetTrigger("isTakingDamage");
            isTakingDamage = true;
            
            if(transform.position.x - Player.Instance.transform.position.x >= 0)
                rb.AddForce(new Vector2(knockbackForce, 1), ForceMode2D.Impulse);
            else 
                rb.AddForce(new Vector2(-knockbackForce, 1), ForceMode2D.Impulse);
            
            AudioManager.Instance.PlaySFX(sfxEnemyHurtList[UnityEngine.Random.Range(0, sfxEnemyHurtList.Count)]);
        }

        if(health <= 0)
        {
            Die();
            AudioManager.Instance.PlaySFX(sfxEnemyDead);
        }
    }

    private void Die()
    {
        onEnemyDie?.Invoke(gameObject);
        anim.SetTrigger("isDead");
        gameObject.layer = 3;
        isDead = true;
        Destroy(gameObject, 1f);
    }
}
