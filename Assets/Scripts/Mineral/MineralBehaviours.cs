using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralBehaviours : MonoBehaviour
{
    private float health;
    private SpriteRenderer sr;
    private Animator anim;

    [HideInInspector] public SO_Mineral mineralData;
    [SerializeField] private float addMineralForce = 1;
    [SerializeField] private List<AudioClip> sfxBeingMined;
    [SerializeField] private AudioClip sfxBroken;

    public void UpdateMineral()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        health = mineralData.Health;
        sr.sprite = mineralData.ItemSprite;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("isBroken");
        if(health > 0)
            AudioManager.Instance.PlaySFX(sfxBeingMined[Random.Range(0, sfxBeingMined.Count)]);
        else
            AudioManager.Instance.PlaySFX(sfxBroken);

        if(health <= 0)
        {
            int randomNumber = Random.Range(mineralData.MinimumSpawnNumber, mineralData.MaximumSpawnNumber + 1);
            for(int i = 0; i < randomNumber; i++)
            {
                GameObject obj = Instantiate(mineralData.DropMineral, transform.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 1f)) * addMineralForce, ForceMode2D.Impulse);

                //Destroy object after an amount of time if it is not collected
                Destroy(obj, 180);
            }
            
            Destroy(gameObject);
        }
    }
}
