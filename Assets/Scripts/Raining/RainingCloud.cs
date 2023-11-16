using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainingCloud : MonoBehaviour
{
    public GameObject rainDrop;
    private float time = 0;

    public GameObject rainingSystem;
    private RainingSystem rainingSystemScript;

    void Start()
    {
        rainingSystemScript = rainingSystem.GetComponent<RainingSystem>();
    }

    void Update()
    {
        if(rainingSystemScript.isRaining)
            Invoke("OnRain", 5);
    }

    void OnRain()
    {
        time = Mathf.MoveTowards(time, 1, Time.deltaTime);

        if(time > Random.Range(0.03f, 0.18f))
        {
            time = 0;
            Instantiate(rainDrop, new Vector3(transform.position.x + Random.Range(-transform.localScale.x, transform.localScale.x)/2, transform.position.y, 2), Quaternion.identity);
        }
    }
}
