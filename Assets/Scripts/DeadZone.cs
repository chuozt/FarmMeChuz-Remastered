using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Transform spawnPoint;
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.name == "Player")
        {
            col.gameObject.transform.position = spawnPoint.position;
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -15), ForceMode2D.Impulse);
        }
    }
}
