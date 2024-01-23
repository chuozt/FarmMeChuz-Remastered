using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingArea : MonoBehaviour
{
    public bool isInShoppingArea = false;

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
            isInShoppingArea = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
            isInShoppingArea = false;
    }
}
