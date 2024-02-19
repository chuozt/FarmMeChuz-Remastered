using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviours : MonoBehaviour
{
    private float health;
    [SerializeField] private SO_Tree treeData;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;
    [SerializeField] private float addLogForce = 1;
    [SerializeField] private GameObject nextTreeGen;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask treeZoneLayer;
    [SerializeField] private List<AudioClip> sfxList_BeingChopped;

    void OnEnable()
    {
        health = treeData.Health;
    }

    public void TreeGrowUp()
    {
        int randomNumber = Random.Range(0,2);
        if(randomNumber == 1)
        {
            if(nextTreeGen != null)
            {
                GameObject tree = Instantiate(nextTreeGen, transform.position, Quaternion.identity);
                GameObject treeZone = tree.transform.GetChild(0).gameObject;
                if(CheckCollisionWithGround(treeZone) && CheckCollisionWithOtherTrees(treeZone))
                    Destroy(gameObject);
                else if(!CheckCollisionWithGround(treeZone) || !CheckCollisionWithOtherTrees(treeZone))
                    Destroy(tree);
            }
        }
    }

    bool CheckCollisionWithGround(GameObject treeZone)
    {
        Collider2D col = Physics2D.OverlapBox(new Vector2(treeZone.transform.position.x, treeZone.transform.position.y + 0.1f), treeZone.transform.localScale, 0, groundLayer);
        if(col == null)
            return true;
        else
            return false;
    }

    bool CheckCollisionWithOtherTrees(GameObject treeZone)
    {
        Collider2D[] col = Physics2D.OverlapBoxAll(new Vector2(treeZone.transform.position.x, treeZone.transform.position.y + 0.1f), treeZone.transform.localScale, 0, treeZoneLayer);
        if(col.Length <= 2)
            return true;
        else
            return false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("isChopped");
        AudioManager.Instance.PlaySFX(sfxList_BeingChopped[Random.Range(0, sfxList_BeingChopped.Count)]);

        if(health <= 0)
        {
            for(int i = 0; i < treeData.Log.Count; i++)
            {
                int randomNumber = Random.Range(treeData.Log[i].MinimumSpawnNumber, treeData.Log[i].MaximumSpawnNumber + 1);
                for(int j = 0; j < randomNumber; j++)
                {
                    GameObject obj = Instantiate(treeData.Log[i].DropLog, new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z), Quaternion.identity);
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 1f)) * addLogForce, ForceMode2D.Impulse);

                    //Destroy object after an amount of time if it is not collected
                    Destroy(obj, 180);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
