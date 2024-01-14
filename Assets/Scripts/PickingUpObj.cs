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
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

                rb.gravityScale = 0f;
                rb.mass = 0f;
                rb.isKinematic = true;
                col.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
                col.gameObject.transform.position = Vector3.MoveTowards(col.gameObject.transform.position, lerpPos.position, 0.16f);
            }
        }
    }
}
