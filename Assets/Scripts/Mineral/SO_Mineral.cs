using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Mineral : ScriptableObject
{
    [field: SerializeField]
    public float Health { get; set; }

    [field: SerializeField]
    public Sprite ItemSprite { get; set; }

    [field: SerializeField]
    public GameObject DropMineral { get; set; }

    [field: SerializeField]
    public int MinimumSpawnNumber { get; set; }

    [field: SerializeField]
    public int MaximumSpawnNumber { get; set; }

    [field: SerializeField] 
    public float PercentageIncreasement { get; set; }

    [field: SerializeField]
    public bool IsSpecialMineral { get; set; }
}