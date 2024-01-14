using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [field: SerializeField]
    public bool IsUnlocked { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite ItemSprite { get; set; }

    [field: SerializeField]
    public bool IsStackable{ get; set; }

    [field: SerializeField]
    public int MaxStackSize{ get; set; } = 1;

    [field: SerializeField]
    public int Cost { get; set; }

    [field: SerializeField]
    public int Value { get; set; }

    [TextAreaAttribute]
    public string Descriptions;

    [field: SerializeField]
    public ItemType ItemType;

    [field: SerializeField]
    public bool IsFuelResource { get; set; }

    [field: SerializeField]
    public float FuelAmount { get; set; }
}

public enum ItemType
{
    Crop = 0,
    CropSeed,
    Mineral,
    Tools,
    None,
};