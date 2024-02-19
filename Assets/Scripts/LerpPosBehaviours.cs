using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosBehaviours : MonoBehaviour
{
    public List<AudioClip> sfx;
    private Item item;

    void OnTriggerStay2D(Collider2D col)
    {
        //check if correct type
        if(col.gameObject.CompareTag("Item"))
        {
            //check if can be picked, then destroy objects, get the info and add to the inventory
            if(col.gameObject.GetComponent<CanBePicked>().canBePicked)
            {
                item = col.gameObject.GetComponent<ObjectsDatas>().item;
                InventoryManager.Instance.AddItem(item);
                AudioManager.Instance.PlaySFX(sfx[Random.Range(0, sfx.Count)]);

                Destroy(col.gameObject);
            }
        }
    }
}
