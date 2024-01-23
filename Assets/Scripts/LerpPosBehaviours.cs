using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosBehaviours : MonoBehaviour
{
    public List<AudioClip> sfx;
    AudioManager audioManager;
    InventoryManager inventoryManager;
    private Item item;
    
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //check if correct type
        if(col.gameObject.CompareTag("Item"))
        {
            //check if can be picked, then destroy objects, get the info and add to the inventory
            if(col.gameObject.GetComponent<CanBePicked>().canBePicked)
            {
                item = col.gameObject.GetComponent<ObjectsDatas>().item;
                inventoryManager.AddItem(item);
                audioManager.PlaySFX(sfx[Random.Range(0, sfx.Count)]);

                Destroy(col.gameObject);
            }
        }
    }
}
