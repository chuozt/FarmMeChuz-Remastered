using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    [field: SerializeField]
    public float Health { get; set; }

    [field: SerializeField]
    public float Damage { get; set; }

    [field: SerializeField]
    public float Speed { get; set; }

    [field: SerializeField]
    public float KnockBackForce { get; set; }

    [field: SerializeField]
    public float DelayBeforeAttack { get; set; }

    [field: SerializeField]
    public float DelayAfterAttack { get; set; }
}
