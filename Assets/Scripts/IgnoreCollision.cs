using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public List<CollisionTypes> collisionTypesIgnore;
    void Awake()
    {
        foreach(CollisionTypes col in collisionTypesIgnore)
        {
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, (int)col);
        }
    }
}

[HideInInspector]
public enum CollisionTypes
{
    DeadEnemy = 3,
    Player = 7,
    Enemy,
    Items = 12,
    Rain,
}
