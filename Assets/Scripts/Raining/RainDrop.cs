using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDrop : MonoBehaviour
{
    public LayerMask groundLayer;
    private Animator anim;
    private Rigidbody2D rb;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapBox(transform.position, new Vector2(0.04227556f, 0.2355783f), 0, groundLayer))
        {
            rb.gravityScale = 0;
            rb.mass = 0;
            rb.velocity = new Vector2(0,0);
            anim.enabled = true;
            Destroy(this.gameObject, 0.25f);
        }
        
        Destroy(this.gameObject, 3);
    }
}
