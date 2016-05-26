using UnityEngine;
using System.Collections;

public class AddPowerProp : Prop
{
    protected override void work()
    {
        this.mActor.changeBombPower(1);
    }

    protected override void removeEffect()
    {
        this.mActor.changeBombPower(-1);
    }
}
