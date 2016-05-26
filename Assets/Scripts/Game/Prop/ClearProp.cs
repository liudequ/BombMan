using UnityEngine;
using System.Collections;

public class ClearProp : Prop
{

    protected override void work()
    {
        Prop[] props = this.mActor.GetComponentsInChildren<Prop>();
        for (int index = 0; index < props.Length; index++)
        {
            var prop = props[index];
            if(prop!=this) prop.notWork();
        }
    }

    protected override void removeEffect()
    {

    }
}
