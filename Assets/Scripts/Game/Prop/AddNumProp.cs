using UnityEngine;
using System.Collections;

public class AddNumProp : Prop
{
    protected override void work()
    {
        this.mActor.changeBombNum(1);
    }

    protected override void removeEffect()
    {
        this.mActor.changeBombNum(-1);
    }
}
