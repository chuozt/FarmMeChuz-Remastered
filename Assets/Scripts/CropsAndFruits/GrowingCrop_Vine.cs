using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCrop_Vine : GrowingCrop_ParentClass
{
    int harvestCounter = 0;
    int randomHarvestNumber;

    void Awake() => randomHarvestNumber = Random.Range(3,5);

    protected override void Havest()
    {
        harvestCounter++;

        base.Havest();
        timeLeft = cropSeedData.TimeToGrow * 2/3;
        canBeHavested = false;

        progressIndex = 1;
        SetStage(progressIndex);

        if(harvestCounter == randomHarvestNumber)
            Destroy(this.gameObject);
    }
}
