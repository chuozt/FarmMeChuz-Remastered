using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemMineral")]
public class ItemMineral : Item
{
    [Header("Item Mineral")]
    [field: SerializeField] public Item Output { get; set; }
    [field: SerializeField] public int OutputNumber { get; set; }
    [field: SerializeField] public float FuelNeed { get; set; }
}