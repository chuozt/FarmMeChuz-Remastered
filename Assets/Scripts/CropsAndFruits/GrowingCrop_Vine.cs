using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCrop_Vine : GrowingCrop_ParentClass
{
    protected override void Havest()
    {
        base.Havest();
        timeLeft = cropSeedData.TimeToGrow * 2/3;
        canBeHavested = false;

        progressIndex = 1;
        SetStage(progressIndex);
    }
}
