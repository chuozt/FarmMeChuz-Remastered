using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDrop : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.mass = 0;
            rb.velocity = new Vector2(0,0);
            rb.isKinematic = true;
            rb.simulated = false;
            anim.enabled = true;
            Destroy(gameObject, 0.25f);
        }
    }
}
