using System;
using UnityEngine;
using System.Collections;
using aStar;
using behaviac;

/// <summary>
/// 游戏中砖块
/// </summary>
public abstract class Brick : MonoBehaviour
{
    public static string BrickTag = "Brick";

    [SerializeField]
    protected bool mIsCanThrough;

    [SerializeField]
    protected int life = 1;

    [SerializeField]
    protected TerrainType terrainType;


    [SerializeField]
    protected BoxCollider mCollider;


    [SerializeField]
    protected PropType propType = PropType.None;


    protected Point2D mPoint2D;


    public virtual void Awake()
    {
        // 向map中写入当前点数据
        this.mPoint2D = GameLevelCommon.GetInstance().point3DTo2D(this.transform);
        GameLevelCommon.GetInstance().setTerrainInfo(terrainType, ref this.mPoint2D);
        GameLevelCommon.GetInstance().setRewardInfo(propType, ref this.mPoint2D);



        //random prop
        int random = UnityEngine.Random.Range(0, 5);
        this.propType = (PropType)Enum.Parse(typeof (PropType), random.ToString());
    }



    /// <summary>
    /// 是否可以空墙
    /// </summary>
    public virtual bool isCanThrough
    {
        get { return this.mIsCanThrough; }
        set
        {
            this.mIsCanThrough = value;
            if (this.mIsCanThrough)
            {
                //mytodo collider 
                this.mCollider.enabled = false;
            }
            else
            {
                this.mCollider.enabled = true;
            }
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Bomb.BombTag)
        {
            this.bomb();
        }
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public virtual void bomb()
    {
        this.life--;
        if (this.life <= 0)
        {
            // 删除寻路中的占位
            GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Normal, ref this.mPoint2D);
            this.checkReward();
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 检查当前砖块下是否有奖励
    /// </summary>
    protected void checkReward()
    {
        if (this.propType != PropType.None)
        {
            // 检查当前坐标下是否有奖励
            GameObject bombObj = Resources.Load("Prefabs/"+this.propType.ToString()) as GameObject;
            bombObj = Instantiate(bombObj);
            bombObj.transform.parent = this.transform.parent;
            bombObj.transform.localPosition = this.transform.localPosition;
        }

       
    }

}
