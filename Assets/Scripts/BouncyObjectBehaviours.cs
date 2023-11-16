using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObjectBehaviours : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();    
    }

    void Bounce()
    {
        rb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(2, 7)), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            Invoke("Bounce", Random.Range(1, 3.5f));
        }
    }
}
