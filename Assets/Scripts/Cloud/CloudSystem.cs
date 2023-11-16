using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSystem : MonoBehaviour
{
    public GameObject[] clouds;
    public float cloudSpeed;

    void Start()
    {
        StartCoroutine(SpawnCloud());
    }

    IEnumerator SpawnCloud()
    {
        yield return new WaitForSeconds((float)Random.Range(2,5));
        HandleObject();
        StartCoroutine(SpawnCloud());
    }

    void HandleObject()
    {
        float randomNum = Random.Range(0.6f, 0.8f);
        GameObject obj = Instantiate(clouds[Random.Range(0,3)], new Vector3(transform.position.x, transform.position.y + (float)Random.Range(-transform.localScale.y/2, transform.localScale.y/2), transform.position.z - randomNum), Quaternion.identity);
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z) * randomNum;
        obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * randomNum * cloudSpeed * Time.deltaTime, ForceMode2D.Force);
        obj.GetComponent<SpriteRenderer>().color = new Color(1,1,1, randomNum);
        
    }
}
