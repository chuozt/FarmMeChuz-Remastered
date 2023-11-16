using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemFruit")]
public class ItemFruit : Item
{
    [field: SerializeField]
    public float TimeBetweenStages { get; set; }
}