using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosBehaviours : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private Item item;

    [Space(20)]
    public List<AudioClip> sfx;

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
                AudioSource.PlayClipAtPoint(sfx[(int)Random.Range(0,5)], transform.position, 1.2f);
                Destroy(col.gameObject);
            }
        }
    }
}
