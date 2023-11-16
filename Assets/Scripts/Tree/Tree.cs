using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tree : ScriptableObject
{
    [field: SerializeField]
    public float Health { get; set; }

    [field: SerializeField]
    public List<GameObject> DropLog { get; set; }

    [field: SerializeField]
    public int MinimumSpawnNumber { get; set; }

    [field: SerializeField]
    public int MaximumSpawnNumber { get; set; }
}
