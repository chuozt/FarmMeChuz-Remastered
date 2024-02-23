using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Material m;
    float distance;

    [SerializeField] private float speed;

    void Start()
    {
        m = GetComponent<Renderer>().material;
    }   

    void Update()
    {
        distance += Time.deltaTime * speed;
        m.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}
