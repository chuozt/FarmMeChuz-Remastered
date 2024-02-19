using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public Enemy enemyData;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackForce;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isTakingDamage = false;
    float time = 0;

    private float health;
    private float damage;

    [SerializeField] private List<AudioClip> sfx;

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
            
            if(transform.position.x - GameObject.Find("Player").transform.position.x >= 0)
                rb.AddForce(new Vector2(knockbackForce, 1), ForceMode2D.Impulse);
            else 
                rb.AddForce(new Vector2(-knockbackForce, 1), ForceMode2D.Impulse);
            
            //AudioSource.PlayClipAtPoint(sfx[Random.Range(0, sfx.Count)], transform.position);
        }

        if(health <= 0)
        {
            anim.SetTrigger("isDead");
            gameObject.layer = 3;
            isDead = true;
            Destroy(gameObject, 1f);
        }
    }
}
