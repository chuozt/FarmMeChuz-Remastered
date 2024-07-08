using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviours : MonoBehaviour
{
    public Transform cameraPos;

    void Update() => this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(cameraPos.position.x, cameraPos.position.y, this.transform.position.z), 0.03f);
}
