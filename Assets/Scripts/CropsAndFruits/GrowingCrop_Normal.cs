using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCrop_Normal : GrowingCrop_ParentClass
{
    protected override void Havest()
    {
        base.Havest();
        Destroy(globalCropParticle.gameObject);
        Destroy(this.gameObject);
    }
}
