using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite ItemSprite { get; set; }

    [field: SerializeField]
    public bool IsStackable{ get; set; }

    [field: SerializeField]
    public int MaxStackSize{ get; set; } = 1;

    [field: SerializeField]
    public bool IsUnlocked { get; set; }

    [field: SerializeField]
    public int Cost { get; set; }

    [field: SerializeField]
    public int Value { get; set; }

    [field: SerializeField]
    public float BuffPercentage { get; set; }

    [TextAreaAttribute]
    public string descriptions;

    [field: SerializeField]
    public ItemType itemType;

    [field: SerializeField]
    public Fuel Fuel;

    void OnEnable()
    {
        if(itemType == ItemType.Point)
        {
            Cost = 200;
        }
        BuffPercentage = 0;
    }
}

public enum ItemType
{
    Crop = 0,
    CropSeed,
    Fruit,
    Mineral,
    Tools,
    Point,
    None,
};

[System.Serializable]
public struct Fuel
{
    [field: SerializeField]
    public bool CanAddFuel { get; set; }

    [field: SerializeField]
    public float FuelCanAdd { get; set; }
}