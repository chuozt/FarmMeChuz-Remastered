using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyManager : MonoBehaviour
{
    public List<GameObject> bouncyObj;

    void Start()
    {
        for(int i = 0; i < (int)Random.Range(bouncyObj.Count/2, bouncyObj.Count - 2); i++)
        {
            Instantiate(bouncyObj[Random.Range(0, bouncyObj.Count - 1)],
                        new Vector3(Random.Range(-3f, 14f), transform.position.y + Random.Range(2f, 7f), transform.position.z),
                        Quaternion.Euler(0, 0, Random.Range(0, 180)));
        }
    }
}
