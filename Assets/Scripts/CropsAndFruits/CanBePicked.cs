using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBePicked : MonoBehaviour
{
    public bool canBePicked = false;
    void Awake()
    {
        StartCoroutine(CanBePickedUp());
    }

    IEnumerator CanBePickedUp()
    {
        yield return new WaitForSeconds(1.25f);
        canBePicked = true;
    }
}
