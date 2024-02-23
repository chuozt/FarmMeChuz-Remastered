using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[] materials;
    float[] backgroundSpeed;
    float furthestBackground;

    [SerializeField] private float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backgroundCount = transform.childCount;
        materials = new Material[backgroundCount];
        backgroundSpeed = new float[backgroundCount];
        backgrounds = new GameObject[backgroundCount];

        for(int i = 0; i < backgroundCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackgroundSpeedCalculate(backgroundCount);
    }

    void BackgroundSpeedCalculate(int backgroundCount)
    {
        for(int i = 0; i < backgroundCount; i++)
        {
            if(backgrounds[i].transform.position.z - cam.position.z > furthestBackground)
                furthestBackground =  backgrounds[i].transform.position.z - cam.position.z;
        }

        for(int i = 0; i < backgroundCount; i++)
            backgroundSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / furthestBackground;
    }

    void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);

        for(int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backgroundSpeed[i] * parallaxSpeed;
            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
