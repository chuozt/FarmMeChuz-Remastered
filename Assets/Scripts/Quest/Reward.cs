using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu]
public class Reward : ScriptableObject
{
    [field:SerializeField] public int Coin { get; set; }
    [field:SerializeField] public int Point { get; set; }
    [field:SerializeField] public List<Item> ItemsUnlock { get; set; }
}
