using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemCrop")]
public class ItemCrop : Item
{
    [field: SerializeField]
    public int HealthIncreaseAmount { get; set; }

    [field: SerializeField]
    public int ManaIncreaseAmount { get; set; }
}