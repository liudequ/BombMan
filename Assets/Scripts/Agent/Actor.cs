using UnityEngine;
using aStar;
using behaviac;
using Debug = UnityEngine.Debug;

[behaviac.TypeMetaInfo()]
public abstract class Actor : behaviac.Agent
{
    public static string ActorTag = "Actor";

    [SerializeField]
    protected string btName = "BombMan";

    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField]
    protected float moveSpeed = 1;

    /// <summary>
    /// 当前可放置的炸弹数量
    /// </summary>
    [SerializeField]
    protected int haveBombNum = 0;

    /// <summary>
    /// 当前可放置的最大炸弹数量
    /// </summary>
    [SerializeField]
    protected int maxHaveBombNum = 0;

    /// <summary>
    /// 炸弹威力
    /// </summary>
    [SerializeField]
    protected int bombPower = 1;

    /// <summary>
    /// 角色生命值
    /// </summary>
    [SerializeField]
    protected int life = 1;

    /// <summary>
    /// Actor朝向
    /// </summary>
    [behaviac.MemberMetaInfo]
    protected DirectionType towards = DirectionType.Down;

    /// <summary>
    /// 当前2D坐标
    /// </summary>
    protected Point2D mPoint2D;


    /// <summary>
    /// 是否无敌
    /// </summary>
    protected bool isDefense = false;

    private bool isInit;


    public void Start()
    {
        if (string.IsNullOrEmpty(btName))
        {
            Debug.LogError("Don't set the behavior tree name!");
        }
        this.btload(this.btName);
        this.btsetcurrent(this.btName);
        this.isInit = true;
        this.mPoint2D = GameLevelCommon.GetInstance().point3DTo2D(this.transform);

       
    }





    /// <summary>
    /// 恢复炸弹
    /// </summary>
    public void recoverBomb()
    {
        this.haveBombNum++;
        if (this.haveBombNum > this.maxHaveBombNum)
        {
            Debug.LogError("the bomb num is Error!!!");
            this.haveBombNum--;
        }
    }

    public virtual void Update()
    {
        if (this.isInit)
            this.btexec();
    }


    [behaviac.MethodMetaInfo()]
    protected virtual EBTStatus move()
    {
        Direction direction = Direction.MoveDirection[(int)this.towards];
        Point2D newPoint = this.mPoint2D + new Point2D(direction.x, direction.y);
        if (GameLevelCommon.GetInstance().isArrive(ref newPoint))
        {
            GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Normal, ref this.mPoint2D);
            this.mPoint2D = newPoint;
            Vector3 position = GameLevelCommon.GetInstance().point2DTo3D(ref this.mPoint2D);
            this.transform.localPosition = position;
            GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Actor, ref this.mPoint2D);
            return EBTStatus.BT_SUCCESS;
        }
        else
        {
            return EBTStatus.BT_FAILURE;
        }
    }


    [behaviac.MethodMetaInfo()]
    protected virtual EBTStatus turnTowards(DirectionType dir)
    {
        int angle = (int)dir * 90;
        this.transform.localEulerAngles = new Vector3(0, angle);
        this.towards = dir;
        return EBTStatus.BT_SUCCESS;
    }


    [behaviac.MethodMetaInfo()]
    protected virtual EBTStatus setBomb()
    {
        if (this.haveBombNum > 0)
        {
            this.haveBombNum--;

            GameObject bombObj = Resources.Load("Prefabs/Bomb") as GameObject;
            bombObj = Instantiate(bombObj);
            bombObj.transform.parent = this.transform.parent;
            bombObj.transform.localPosition = this.transform.localPosition;
            Bomb bomb = bombObj.GetComponent<Bomb>();
            bomb.set(this);
        }
        return EBTStatus.BT_SUCCESS;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Bomb.BombTag)
        {
            if (!this.isDefense)
            {
                this.life--;
                if (this.life <= 0)
                {
                    GameLevelCommon.GetInstance().setTerrainInfo(TerrainType.Normal, ref this.mPoint2D);
                    GameObject.Destroy(this.gameObject);
                } 
            }
        }
        else if (other.gameObject.tag == Prop.PropTag)
        {
            Prop prop = other.GetComponent<Prop>();
            prop.append(this);
        }
    }




    #region get

    /// <summary>
    /// 炸弹威力
    /// </summary>
    /// <returns></returns>
    public int getBombPower()
    {
        return this.bombPower;
    }
    #endregion

    #region 奖励效果

    public void changeBombNum(int num)
    {
        this.haveBombNum += num;
        this.maxHaveBombNum += num;
    }

    public void changeSpeed(float speed)
    {
        this.moveSpeed += speed;
    }

    public void changeBombPower(int power)
    {
        this.bombPower += power;
    }


    public void changeDefense(bool defense)
    {
        this.isDefense = defense;
    }

    #endregion
}
