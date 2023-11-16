using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapes : MonoBehaviour
{
    public Renderer fruit1;
    public Renderer fruit2;
    public Renderer fruit3;
    public Renderer fruit4;
    public GameObject fruitObj; 
    public Animator bushAnimator;

    [Space(10)]
    public ItemFruit doneFruit;
    public float timeBetweenStages;

    [Space(20)]
    public AudioClip sfx;

    [Space(20)]
    public List<GameObject> particles;
    public float particleForce;

    void Awake()
    {
        timeBetweenStages = doneFruit.TimeBetweenStages - (doneFruit.TimeBetweenStages * doneFruit.BuffPercentage/100);
        StartCoroutine(GrowingStages());
    }
    
    void Update()
    {
        timeBetweenStages = doneFruit.TimeBetweenStages - (doneFruit.TimeBetweenStages * doneFruit.BuffPercentage/100);
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
        Growing(fruit1);
        float random = Random.Range(timeBetweenStages - 2, timeBetweenStages + 2);

        yield return new WaitForSeconds(random);
        Growing(fruit2);

        yield return new WaitForSeconds(random);
        Growing(fruit3);

        yield return new WaitForSeconds(random);
        Growing(fruit4);

        yield return new WaitForSeconds(random);
        fruit4.enabled = false;

        HandleObject();

        //Play animation and SFX
        bushAnimator.SetTrigger("Bush");
        AudioSource.PlayClipAtPoint(sfx, this.transform.position);
        
        SpawningParticles();
        yield return new WaitForSeconds(random);
        StartCoroutine(GrowingStages());
    }

    void HandleObject()
    {
        GameObject obj = Instantiate(fruitObj, this.transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.1f, 0.1f), 0.1f) * 6.5f, ForceMode2D.Impulse);
        Destroy(obj, 180);
    }

    void SpawningParticles()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject particle = Instantiate(particles[Random.Range(0, 3)], 
                                            new Vector3(transform.position.x + (float)Random.Range(-0.3f, 0.3f),
                                            transform.position.y + (float)Random.Range(-0.2f, 0.3f),
                                            transform.position.z - 2), 
                                            Quaternion.identity);
            particle.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)Random.Range(-3f, 3f), Random.Range(2f, 4.5f)) * particleForce);
            Destroy(particle, 10);
        }
    }
}
