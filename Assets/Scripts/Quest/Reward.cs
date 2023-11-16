using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu]
public class Reward : ScriptableObject
{
    [field:SerializeField]
    public int coin;

    [field:SerializeField]
    public int point;

    [field:SerializeField]
    public List<Item> ItemsUnlock { get; set; }
}
