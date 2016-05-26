using UnityEngine;
using System.Collections;


public class LawnBrick : Brick
{

    /// <summary>
    /// 是否可以空墙
    /// </summary>
    public override bool isCanThrough
    {
        get { return this.mIsCanThrough; }
        set
        {
            this.mCollider.enabled = this.mIsCanThrough = true;
        }
    }

    public override void Awake()
    {
        base.Awake();
        this.isCanThrough = true;
    }
    
    

    public override void bomb()
    {
        
    }
}
