using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemChildren/ItemTool")]
public class ItemTool : Item
{
    [field: SerializeField, Header("Item Tool")]
    public ToolType toolType;

    [field: SerializeField]
    public float Delay { get; set; }

    [field: SerializeField]
    public float Damage { get; set; }
}

public enum ToolType
{
    Pickaxe,
    Sword,
    Axe,
    Shovel,
    Bucket
};