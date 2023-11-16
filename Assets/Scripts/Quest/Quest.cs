using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu]
public class Quest : ScriptableObject
{
    [field:SerializeField]
    public int Level { get; set; }

    [field:SerializeField]
    public List<ItemsNeed> ItemsNeed;

    [field:SerializeField]
    public List<bool> IsDone { get; set; }

}

[System.Serializable]
public struct ItemsNeed
{
    [field:SerializeField]
    public Item ItemNeed { get; set; }

    [field:SerializeField]
    public int NumbersOfItemNeed { get; set; }
}
