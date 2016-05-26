using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;


public enum PropType
{
    None = 0,
    AddPower,
    AddNum,
    Defense,
    Clear,

}


/// <summary>
/// 游戏中道具
/// </summary>
public abstract class Prop : MonoBehaviour
{

    public static string PropTag = "Prop";

    /// <summary>
    /// 持续时间 -1代表一直持续
    /// </summary>
    public float lifeTime = 0;

    /// <summary>
    /// 是否叠加
    /// </summary>
    public bool isOverlay = true;

    /// <summary>
    /// 当前作用的对象
    /// </summary>
    protected Actor mActor;

    /// <summary>
    /// 加到某个角色上
    /// </summary>
    /// <param name="actor"></param>
    public void append(Actor actor)
    {
        this.mActor = actor;
        if (!this.isOverlay)
        {
            Prop old = this.mActor.GetComponentInChildren(this.GetType()) as Prop;
            if (old != null)
            {
                old.notWork();
            }
        }
        this.transform.parent = this.mActor.transform;
        this.transform.localPosition = Vector3.one*1000;
        this.work();
    }

    public void Update()
    {
        if (mActor != null && this.lifeTime != -1 && this.lifeTime > 0) //倒计时
        {
            this.lifeTime -= Time.deltaTime;
            if (this.lifeTime <= 0)
            {
                this.notWork();
            }
        }
    }

    public virtual void OnDestroy()
    {
        if (this.mActor != null)
        {
            this.mActor = null;
        }
    }



    /// <summary>
    /// 起作用
    /// </summary>
    protected abstract void work();



    public void notWork()
    {
        this.removeEffect();
        this.mActor = null;
        //mytodo recyle
        GameObject.Destroy(this.gameObject);
    }


    /// <summary>
    /// 移除效果
    /// </summary>
    protected abstract void removeEffect();


}
