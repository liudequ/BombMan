using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aStar;
using behaviac;

[behaviac.TypeMetaInfo("Player", "Player -> Actor")]
public class Player : Actor
{

    [SerializeField]
    private bool isAI = false;

    public override void Update()
    {
        if (this.isAI)
        {
            base.Update();
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.keyDown(DirectionType.Down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.keyDown(DirectionType.Up);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.keyDown(DirectionType.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.keyDown(DirectionType.Right);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            base.setBomb();
        }
    }

    private void keyDown(DirectionType type)
    {
        if (this.towards == type)
        {
            base.move();
        }
        else
        {
            base.turnTowards(type);
        }
    }

    [behaviac.MemberMetaInfo()]
    protected List<AStarPosition> path = null;


    [behaviac.MemberMetaInfo()]
    protected bool isNavigate = false;




    /// <summary>
    /// 获取可以走的方向
    /// </summary>
    /// <returns></returns>
    [behaviac.MethodMetaInfo()]
    public DirectionType getMoveDirection()
    {
        GameLevelCommon common = GameLevelCommon.GetInstance();
        Point2D[] rang = common.getRang(1, ref this.mPoint2D, false);
        for (int index = 0; index < rang.Length; index++)
        {
            var point2D = rang[index];
            if (common.isArrive(ref point2D))
            {
                return common.relativePosition(ref this.mPoint2D, ref point2D);
            }
        }
        return DirectionType.Origin;
    }


    /// <summary>
    /// 当前是否可以放炸弹
    /// </summary>
    /// <returns></returns>
    [behaviac.MethodMetaInfo()]
    public EBTStatus isCanSetBomb()
    {
        if (this.haveBombNum <= 0) return EBTStatus.BT_FAILURE;

        GameLevelCommon common = GameLevelCommon.GetInstance();
        Point2D[] points = common.getBombRang(this.bombPower, ref this.mPoint2D);

        for (int index = 0; index < points.Length; index++)
        {
            var point2D = points[index];
            if (common.isHaveValueForBomb(ref point2D))
                return EBTStatus.BT_SUCCESS;
        }
        return EBTStatus.BT_FAILURE;
    }



    [behaviac.MethodMetaInfo()]
    public EBTStatus isDanger()
    {
        bool isDanger = GameLevelCommon.GetInstance().isDangerPosition(ref this.mPoint2D);
        return isDanger ? EBTStatus.BT_SUCCESS : EBTStatus.BT_FAILURE;
    }




    [behaviac.MethodMetaInfo()]
    public EBTStatus findSavePoint()
    {
        GameLevelCommon common = GameLevelCommon.GetInstance();
        AStarPathFind pathFind = AStarPathFind.instance;

        List<Point2D> seek = new List<Point2D>();

        Direction upDir = Direction.GetDirenction(DirectionType.Up);
        Direction leftDir = Direction.GetDirenction(DirectionType.Left);
        Direction rightDir = Direction.GetDirenction(DirectionType.Right);

        Point2D leftPoint2D, rightPoint2D;

        int xInterval = 1;

        for (int i = 1; i < 10; i++) //i次找不到就不找了。
        {
            xInterval = 1; //该值代表点X轴距mPoint2d的距离
            seek.Clear();

            //1.获得点,根据计算最后获得的是当前点移动i步后的安全位置集合(菱形)
            Point2D[] range = common.getRang(i, ref this.mPoint2D, false);

            Point2D downPoint = range[range.Length - 1];

            if (this.isCanGo(ref common, ref downPoint))
                seek.Add(downPoint);

            do
            {
                downPoint += upDir;

                rightPoint2D = leftPoint2D = downPoint;
                for (int j = 0; j < xInterval; j++)
                {
                    leftPoint2D += leftDir;
                    rightPoint2D += rightDir;
                }

                if (this.isCanGo(ref common, ref leftPoint2D))
                    seek.Add(leftPoint2D);

                if (xInterval != 0 && this.isCanGo(ref common, ref rightPoint2D)) //不是顶点位置,如果顶点只有一个
                {
                    seek.Add(rightPoint2D);
                }

                DirectionType relativeDir = common.relativePosition(ref downPoint, ref mPoint2D);

                if (relativeDir == DirectionType.Up)
                {
                    xInterval++;
                }
                else
                {
                    xInterval--;
                }

            } while (xInterval >= 0);

            //3.通过Astar判断是否可达

            for (int index = 0; index < seek.Count; index++)
            {
                var point2D = seek[index];
                this.path = pathFind.findPath(this.mPoint2D.x, this.mPoint2D.y, point2D.x, point2D.y);
                if (this.path == null || this.path.Count == 0)
                    continue;
                return EBTStatus.BT_SUCCESS;
            }

            //4.如果位置集合都不满足，则扩大位置
        }

        return EBTStatus.BT_FAILURE;
    }

    /// <summary>
    /// //2.判断是否为可达并不危险点
    /// </summary>
    /// <param name="common"></param>
    /// <param name="point2d"></param>
    /// <returns></returns>
    private bool isCanGo(ref GameLevelCommon common, ref Point2D point2d)
    {
        if (common.isArrive(ref point2d) && !common.isDangerPosition(ref point2d))
            return true;
        return false;
    }


    [behaviac.MethodMetaInfo()]
    public EBTStatus navigate()
    {
        if (this.path == null)
            return EBTStatus.BT_FAILURE;
        if (this.path.Count == 0)
            return EBTStatus.BT_SUCCESS;
        if (this.isNavigate)
            return EBTStatus.BT_RUNNING;
        this.StartCoroutine(moveToTarget());
        return EBTStatus.BT_RUNNING;
    }


    private IEnumerator moveToTarget()
    {
        this.isNavigate = true;
        for (int index = 1; index < this.path.Count; index++)
        {
            var aStarPosition = this.path[index];
            this.turnTowards(aStarPosition.dir);
            this.move();
            yield return new WaitForSeconds(0.5f);
        }
        this.path.Clear();
        this.isNavigate = false;
    }
}
