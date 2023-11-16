using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTrees : MonoBehaviour
{
    private Animator anim;
    public bool isTriggering = false;
    public GameObject whatFruit;
    public GameObject whiteBorder;

    public AudioClip sfx;

    void Start()
    {
        anim = GetComponent<Animator>();
        whiteBorder.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    void Update()
    {
        //if is standing under the tree, the fruit can be collected and press F, play the animation and disable the white border
        if(isTriggering && Input.GetKeyDown(KeyCode.F) && whatFruit.GetComponent<BigTreesFruits>().canBeCollected)
        {
            whiteBorder.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(ShakedTree());
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player" && whatFruit.GetComponent<BigTreesFruits>().canBeCollected)
        {
            isTriggering = true;
            whiteBorder.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) 
    {
        if(col.gameObject.name == "Player")
        {
            isTriggering = false;
            whiteBorder.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator ShakedTree()
    {
        anim.SetTrigger("isShaked");
        AudioSource.PlayClipAtPoint(sfx, this.transform.position, 10f);
        yield return new WaitForSeconds(0.22f);
        anim.SetTrigger("isIdle");
    }
}
