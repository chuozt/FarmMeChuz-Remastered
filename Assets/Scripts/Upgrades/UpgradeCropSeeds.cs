using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/UpgradeCropSeeds")]
public class UpgradeCropSeeds : UpgradeCategoryData
{
    public List<ItemCropSeed> cropSeeds;

    public override void ApplyUpgrade()
    {
        foreach(var cropSeed in cropSeeds)
        {
            //cropSeed.BuffPercentage += percentage;
        }
    }
}
