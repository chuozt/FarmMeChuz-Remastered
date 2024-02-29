using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnerTransform;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    EnemyStats enemyStats;

    bool isIdle = true;
    bool isRunning = false;
    bool isDetecting = false;
    bool isFighting = false;
    [SerializeField] private float idleTime;
    [SerializeField] private float runningTime;
    [SerializeField] private float fightingTime;
    [SerializeField] private Transform detectBoxPoint;
    [SerializeField] private Vector2 detectBoxSize;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private Transform hitboxPoint;
    [SerializeField] private float hitboxRadius;

    float currentIdleTime = 0;
    float currentRunningTime = 0;
    float currentDelayBeforeAttackTime = 0;
    float currentDelayAfterAttackTime = 0;
    float time = 0;

    bool canFight = true;
    bool isFacingRight = true;
    bool isFacingLeft = false;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        currentIdleTime = Random.Range(0, 1.5f);
        currentRunningTime = Random.Range(0, 1.5f);
    }

    void Update()
    {
        if(!enemyStats.isDead && !enemyStats.isTakingDamage)
        {
            if(!DelayState())
            {
                if(!DetectPlayerState())
                {
                    currentIdleTime = Mathf.MoveTowards(currentIdleTime, idleTime, Time.deltaTime);
                    currentRunningTime = Mathf.MoveTowards(currentRunningTime, runningTime, Time.deltaTime);

                    FlipXState();
                    if(isRunning)
                    {
                        MoveState();
                    }

                    if(currentIdleTime == idleTime)
                    {
                        isRunning = false;
                        anim.SetTrigger("isIdle");
                        currentIdleTime = Random.Range(0, 1.5f);
                    }

                    if(currentRunningTime == runningTime)
                    {
                        isRunning = true;
                        anim.SetTrigger("isRunning");  
                        currentRunningTime = Random.Range(0, 1.5f);
                    }
                }
            }

            anim.SetBool("isDetecting", isDetecting);

            if(isFighting || !canFight)
                ResetAttack();
        }
    }

    void MoveState()
    {
        if(isFacingRight)
        {
            transform.position += new Vector3(enemyStats.enemyData.Speed, 0, 0) * Time.deltaTime;
        }
        else if(isFacingLeft)
        {
            transform.position += new Vector3(-enemyStats.enemyData.Speed, 0, 0) * Time.deltaTime;
        }
    }

    void SetFacingRight()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = true;
        isFacingLeft = false;
    }

    void SetFacingLeft()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = false;
        isFacingLeft = true;
    }

    void FlipXState()
    {
        if(transform.position.x <= enemySpawnerTransform.position.x + (-enemySpawnerTransform.localScale.x/2) - 0.5f && isFacingLeft)
            SetFacingRight();
        else if(transform.position.x >= enemySpawnerTransform.position.x + (enemySpawnerTransform.localScale.x/2) + 0.5f && isFacingRight)
            SetFacingLeft();
    }

    bool DetectPlayerState()
    {
        Collider2D col = Physics2D.OverlapBox(detectBoxPoint.position, detectBoxSize, 0, detectLayer);
        if(col != null)
        {
            isDetecting = true;
            if(!isRunning)
            {
                anim.SetTrigger("isRunning");
                isRunning = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, enemyStats.enemyData.Speed * Time.deltaTime);
            
            if(transform.position.x < col.transform.position.x && isFacingLeft)
                SetFacingRight();
            else if(transform.position.x >= col.transform.position.x && isFacingRight)
                SetFacingLeft();

            return true;
        }
        isDetecting = false;
        return false;
    }

    bool DelayState()
    {
        Collider2D col = Physics2D.OverlapCircle(hitboxPoint.position, hitboxRadius, detectLayer);

        if(col != null)
        {
            anim.SetTrigger("isIdle");
            isDetecting = false;
            isRunning = false;
            currentDelayBeforeAttackTime = Mathf.MoveTowards(currentDelayBeforeAttackTime, enemyStats.enemyData.DelayBeforeAttack, Time.deltaTime);

            if(currentDelayBeforeAttackTime >= enemyStats.enemyData.DelayBeforeAttack)
            {
                AttackState();
                currentDelayBeforeAttackTime = 0;
            }
            return true;
        }
        return false;
    }

    void AttackState()
    {
        if(!isFighting && canFight)
        {
            anim.SetTrigger("isFighting");
            isFighting = true;
            canFight = false;

            Collider2D col = Physics2D.OverlapCircle(hitboxPoint.position, hitboxRadius * 2, detectLayer);
            {
                if(col != null)
                    col.GetComponent<Player>().TakeDamage((int)enemyStats.enemyData.Damage, this.transform, enemyStats.enemyData.KnockBackForce);
            }
        }
    }

    void ResetAttack()
    {
        time = Mathf.MoveTowards(time, anim.GetCurrentAnimatorStateInfo(0).length, Time.deltaTime);
        if(time >= anim.GetCurrentAnimatorStateInfo(0).length)
        {
            anim.SetTrigger("isIdle");
            isRunning = false;
            isFighting = false;
            time = 0;
        }

        currentDelayAfterAttackTime = Mathf.MoveTowards(currentDelayAfterAttackTime, enemyStats.enemyData.DelayAfterAttack, Time.deltaTime);
        if(currentDelayAfterAttackTime >= enemyStats.enemyData.DelayAfterAttack)
        {
            canFight = true;
            currentDelayAfterAttackTime = 0;
        }
    }

    void OnDrawGizmos()
    {
        if(detectBoxPoint == null)
            return;
        Gizmos.DrawWireCube(detectBoxPoint.position, detectBoxSize);
        Gizmos.DrawWireSphere(hitboxPoint.position, hitboxRadius);
    }
}
