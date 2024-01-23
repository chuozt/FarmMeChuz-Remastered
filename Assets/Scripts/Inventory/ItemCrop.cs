using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ItemChildren/ItemCrop")]
public class ItemCrop : Item
{
    [field: SerializeField, Header("Item Crop")]
    public int HealthIncreaseAmount { get; set; }

    [field: SerializeField]
    public int ManaIncreaseAmount { get; set; }
}