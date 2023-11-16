using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingUpObj : MonoBehaviour
{
    public Transform lerpPos;

    void OnTriggerStay2D(Collider2D col)
    {
        //check if correct type
        if(col.gameObject.CompareTag("Item"))
        {
            //check if can be picked, if yes, set the collision to trigger, move towards to the player
            if(col.gameObject.GetComponent<CanBePicked>().canBePicked)
            {
                col.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                col.gameObject.GetComponent<Rigidbody2D>().mass = 0f;
                col.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
                col.gameObject.transform.position = Vector3.MoveTowards(col.gameObject.transform.position, lerpPos.position, 0.16f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //check if correct type
        if(col.gameObject.CompareTag("Item"))        
        {
            if(col.gameObject.GetComponent<CanBePicked>().canBePicked)
            {
                col.gameObject.transform.position = Vector3.MoveTowards(col.gameObject.transform.position, lerpPos.position, 0.12f);
            }
        }
    }
    
}
