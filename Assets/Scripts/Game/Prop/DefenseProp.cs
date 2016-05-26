using UnityEngine;
using System.Collections;

public class DefenseProp : Prop
{

    private GameObject shieldObj;


    protected override void work()
    {
        this.mActor.changeDefense(true);
        this.shieldObj = Resources.Load<GameObject>("Prefabs/ShieldEffect");
        this.shieldObj = Instantiate(shieldObj);
        this.shieldObj.transform.parent = this.mActor.transform;
        this.shieldObj.transform.localPosition = Vector3.zero;
    }

    protected override void removeEffect()
    {
        this.mActor.changeDefense(false);
        GameObject.Destroy(this.shieldObj);
        this.shieldObj = null;
    }
}
