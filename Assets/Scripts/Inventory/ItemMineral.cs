using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemMineral")]
public class ItemMineral : Item
{
    [field: SerializeField, Header("Item Mineral")]
    public Item Output { get; set; }

    [field: SerializeField]
    public float FuelNeed { get; set; }
}