using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemCropSeed")]
public class ItemCropSeed : Item
{
    [field: SerializeField, Header("Item Crop Seed")]
    public int MinSpawningCrops { get; set; }

    [field: SerializeField]
    public int MaxSpawningCrops { get; set; }  
    
    [field: SerializeField]
    public float TimeToGrow { get; set; }

    [field: SerializeField]
    public GameObject GrowingCrop{ get; set; }

    [field: SerializeField]
    public GameObject DropCrop { get; set; }
}