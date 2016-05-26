using UnityEngine;
using System.Collections;
using aStar;


/// <summary>
/// 炸弹
/// </summary>
public class Bomb : MonoBehaviour
{
    public static string BombTag = "Bomb";


    private float lifeTime = 1;



    /// <summary>
    /// 爆炸效果
    /// </summary>
    [SerializeField]
    private GameObject bombEffect;

    /// <summary>
    /// 是否放置
    /// </summary>
    [SerializeField]
    private bool isSet = false;

    /// <summary>
    /// setter
    /// </summary>
    [SerializeField]
    private Actor mActor;


    /// <summary>
    /// 爆炸范围
    /// </summary>
    private Point2D[] bombRangs;

    private int bombPower;

    private Point2D mPoint;

    public void set(Actor actor)
    {
        this.mPoint = GameLevelCommon.GetInstance().point3DTo2D(this.gameObject.transform);
        this.isSet = true;
        this.mActor = actor;
        this.lifeTime = 1;
        this.bombPower = this.mActor.getBombPower(); //缓存是因为，有可能在爆炸前，玩家数据发生变化
        this.bombRangs = GameLevelCommon.GetInstance().getBombRang(this.bombPower, ref this.mPoint);
        GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Bomb, ref this.mPoint);
        GameLevelCommon.GetInstance().addDangerPreformence(ref this.bombRangs);
    }


    public void Update()
    {
        if (this.mActor != null && this.isSet && this.lifeTime > 0)
        {
            this.lifeTime -= Time.deltaTime;
            if (this.lifeTime <= 0)
            {
                bomb();
            }
        }
    }


    public virtual void bomb()
    {
        // 消除范围内方块及角色
        for (int i = 0; i < this.bombRangs.Length; i++)
        {
            if (!this.bombRangs[i].isVaild)
                continue;
            GameObject effect = GameObject.Instantiate(this.bombEffect);
            effect.transform.parent = this.transform.parent;
            effect.transform.localPosition = GameLevelCommon.GetInstance().point2DTo3D(ref this.bombRangs[i]);
        }
        this.reset();
        GameObject.Destroy(this.gameObject);
    }

  

    public void reset()
    {
        this.lifeTime = 0;
        this.isSet = false;
        if (this.mActor != null)
        {
            this.mActor.recoverBomb();
            this.mActor = null;
        }
        GameLevelCommon.GetInstance().reduceDangerPreformence(ref this.bombRangs);
        this.bombRangs = null;
        GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Normal, ref this.mPoint);
    }



}
