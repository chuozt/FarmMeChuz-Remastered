using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemCropSeed")]
public class ItemCropSeed : Item
{
    [field: SerializeField]
    public int MinSpawningCrops { get; set; }

    [field: SerializeField]
    public int MaxSpawningCrops { get; set; }  
    
    [field: SerializeField]
    public float TimeToGrow { get; set; }

    [field: SerializeField]
    public GameObject GrowingCrop{ get; set; }
}