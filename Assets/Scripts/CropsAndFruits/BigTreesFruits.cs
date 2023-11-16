using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTreesFruits : MonoBehaviour
{
    public GameObject bigTree;
    public Renderer fruit1;
    public Renderer fruit2;
    public Renderer fruit3;
    public Renderer fruit4;
    public GameObject fruitObj;

    private BigTrees bigTreesScript; 
    
    [Space(10)]
    public bool canBeCollected = false;

    [Space(10)]
    public ItemFruit doneFruit;
    public float timeBetweenStages;

    [Space(20)]
    public List<GameObject> particles;
    public float particleForce;

    void Awake()
    {
        //get the time between stages from the scriptable objects
        timeBetweenStages = doneFruit.TimeBetweenStages - (doneFruit.TimeBetweenStages * doneFruit.BuffPercentage/100);
        StartCoroutine(GrowingStages());
        bigTreesScript = bigTree.GetComponent<BigTrees>();
        fruit1.enabled = false;
        fruit2.enabled = false;
        fruit3.enabled = false;
        fruit4.enabled = false;
    }
    
    void Update()
    {
        //Keep update the time between the stages from the scriptable objects, as they are being updated over time
        timeBetweenStages = doneFruit.TimeBetweenStages - (doneFruit.TimeBetweenStages * doneFruit.BuffPercentage/100);

        //if havest, start the process all over again
        if(bigTreesScript.isTriggering && Input.GetKeyDown(KeyCode.F) && canBeCollected)
        {
            fruit4.enabled = false;
            canBeCollected = false;
            
            HandleObject();
            SpawningParticles();
            StartCoroutine(GrowingStages());
        }
    }

    void Growing(Renderer image)
    {
        //Disable all the renderers
        fruit1.enabled = false;
        fruit2.enabled = false;
        fruit3.enabled = false;
        fruit4.enabled = false;

        //Enable the parameter renderer
        image.enabled = true;
    }

    IEnumerator GrowingStages()
    {
        yield return new WaitForSeconds(timeBetweenStages);
        Growing(fruit1);

        yield return new WaitForSeconds(timeBetweenStages);
        Growing(fruit2);

        yield return new WaitForSeconds(timeBetweenStages);
        Growing(fruit3);

        yield return new WaitForSeconds(timeBetweenStages);
        Growing(fruit4);
        canBeCollected = true;
    }

    //Spawn and handle the obj infor
    void HandleObject()
    {
        GameObject obj = Instantiate(fruitObj, this.transform.position, Quaternion.Euler(0,0, Random.Range(-15, 15)));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.15f, 0.15f), 0.6f) * Random.Range(6,9), ForceMode2D.Impulse);
        Destroy(obj, 120);
    }

    void SpawningParticles()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject particle = Instantiate(particles[Random.Range(0, 3)], 
                                            new Vector3(transform.position.x + (float)Random.Range(-0.3f, 0.3f),
                                            transform.position.y + (float)Random.Range(-0.2f, 0.3f),
                                            transform.position.z - 2), 
                                            Quaternion.Euler(0, 0, (float)Random.Range(-90, 90)));
            particle.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)Random.Range(-3f, 3f), Random.Range(2f, 4.5f)) * particleForce);
            Destroy(particle, 10);
        }
    }
}
