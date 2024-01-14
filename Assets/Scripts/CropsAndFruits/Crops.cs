using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour
{
    //Components
    [Space(10)]
    public Renderer stage1;
    public Renderer stage2;
    public Renderer stage3;

    [Space(20)]
    public GameObject doneCropGO;
    bool checkCropFinalStage = false;
    
    //Var
    [Space(10)]
    public ItemCropSeed cropSeedData;
    [HideInInspector] public float timeBetweenStages;

    [Space(20)]
    public List<AudioClip> sfx;

    void Awake()
    {
        SetStage(stage1);
        timeBetweenStages = cropSeedData.TimeToGrow;
        StartCoroutine(Growing());
    }

    void Update()
    {
        if(checkCropFinalStage)
        {
            HandleObject();
        }
    }

    void SetStage(Renderer currentStage)
    {
        //Disable all the renderers
        stage1.enabled = false;
        stage2.enabled = false;
        stage3.enabled = false;

        //Enable the parameter renderer
        currentStage.enabled = true;
    }

    IEnumerator Growing()
    {
        float random = Random.Range(timeBetweenStages - 0.75f, timeBetweenStages + 0.5f);
        
        yield return new WaitForSeconds(random);
        SetStage(stage2);
        yield return new WaitForSeconds(random);
        SetStage(stage3);
        yield return new WaitForSeconds(random);
        checkCropFinalStage = true;
        
        AudioSource.PlayClipAtPoint(sfx[(int)Random.Range(0,4)], transform.position, 1.2f);
    }
    
    void HandleObject()
    {
        stage3.enabled = false;
        int randomNum = Random.Range(cropSeedData.MinSpawningCrops,cropSeedData.MaxSpawningCrops + 1);
        for(int i = 0; i < randomNum; i++)
        {
            GameObject crop = Instantiate(doneCropGO, this.transform.position, Quaternion.Euler(0,0, Random.Range(-15f, 15f)));
            crop.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.3f, 0.3f), 0.3f) * 1.3f, ForceMode2D.Impulse);
            Destroy(crop, 300);
        }

        Destroy(this.gameObject);
    }

}
