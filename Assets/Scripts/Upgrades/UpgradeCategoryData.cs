using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeCategoryData : ScriptableObject
{
    public float percentage;

    public abstract void ApplyUpgrade();
}
