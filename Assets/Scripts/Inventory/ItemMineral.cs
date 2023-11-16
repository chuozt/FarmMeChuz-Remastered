using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemMineral")]
public class ItemMineral : Item
{
    [field: SerializeField]
    public ItemMineral Output { get; set; }
}