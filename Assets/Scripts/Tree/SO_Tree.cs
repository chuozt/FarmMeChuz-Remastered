using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Tree : ScriptableObject
{
    [field: SerializeField] public float Health { get; set; }
    [field: SerializeField] public List<Log> Log { get; set; }
}

[System.Serializable]
public struct Log
{
    [field: SerializeField] public GameObject DropLog { get; set; }
    [field: SerializeField] public int MinimumSpawnNumber { get; set; }
    [field: SerializeField] public int MaximumSpawnNumber { get; set; }
}
