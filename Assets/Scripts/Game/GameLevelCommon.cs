using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using aStar;

public class GameLevelCommon : MonoBehaviour
{
    public static int UnitSize = 4;

    private static GameLevelCommon instance;



    public int SizeWidth;

    public int SizeHeight;


    /// <summary>
    /// 地图信息
    /// </summary>
    private byte[,] mapBytes;


    /// <summary>
    /// 地图中奖励位置信息
    /// </summary>
    private byte[,] propBytes;



    private Dictionary<Point2D, int> dangerPerformance = new Dictionary<Point2D, int>();


    /// <summary>
    /// 游戏范围
    /// </summary>
    private Rect gameRect;


    public static GameLevelCommon GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("GameLevelCommon singleton is null");
            return null;
        }
        return instance;
    }


    public void Awake()
    {
        instance = this;
        this.mapBytes = new byte[SizeWidth, SizeHeight];
        this.propBytes = new byte[SizeWidth, SizeHeight];
        this.gameRect = new Rect(0, 0, SizeWidth, SizeHeight);

    }

    public void Start()
    {
        AStarPathFind aStar = new AStarPathFind(SizeWidth, SizeHeight, this.mapBytes);
    }


    #region 设置地形与奖励信息

    public void setTerrainInfo(TerrainType type, ref Point2D point2d)
    {
        this.mapBytes[point2d.x, point2d.y] = (byte)type;
    }

    public void setTerrainInfo(Transform obj, TerrainType type)
    {
        Point2D point2d = this.point3DTo2D(obj);
        this.setTerrainInfo(type, ref point2d);
    }


    public void setRewardInfo(PropType type, ref Point2D point2d)
    {
        this.propBytes[point2d.x, point2d.y] = (byte)type;
    }

    public void setRewardInfo(Transform obj, PropType type)
    {
        Point2D point2d = this.point3DTo2D(obj);
        this.setRewardInfo(type, ref point2d);
    }

    /// <summary>
    /// 增大危险评估
    /// </summary>
    /// <param name="bombRange"></param>
    public void addDangerPreformence(ref Point2D[] bombRange)
    {
        for (int index = 0; index < bombRange.Length; index++)
        {
            var point2D = bombRange[index];
            if (this.dangerPerformance.ContainsKey(point2D))
            {
                this.dangerPerformance[point2D]++;
            }
            else
            {
                this.dangerPerformance.Add(point2D, 1);
            }
        }
    }

    /// <summary>
    /// 减小危险评估
    /// </summary>
    /// <param name="bombRange"></param>
    public void reduceDangerPreformence(ref Point2D[] bombRange)
    {
        for (int index = 0; index < bombRange.Length; index++)
        {
            var point2D = bombRange[index];
            if (this.dangerPerformance.ContainsKey(point2D))
            {
                this.dangerPerformance[point2D]--;
            }
        }
    }

    #endregion

    /// <summary>
    /// 是否可到达
    /// </summary>
    /// <param name="point2d"></param>
    /// <returns></returns>
    public bool isArrive(ref Point2D point2d)
    {
        if (!this.isPointVaild(ref point2d))
            return false;
        byte typeByte = this.mapBytes[point2d.x, point2d.y];
        TerrainType type = (TerrainType)Enum.ToObject(typeof(TerrainType), typeByte);
        return aStar.Terrain.isArrive(type);
    }


    /// <summary>
    /// 位置是否可炸
    /// </summary>
    /// <param name="point2d"></param>
    /// <returns></returns>
    public bool isBomb(ref Point2D point2d)
    {
        if (!this.isPointVaild(ref point2d))
            return false;
        byte typeByte = this.mapBytes[point2d.x, point2d.y];
        TerrainType type = (TerrainType)Enum.ToObject(typeof(TerrainType), typeByte);
        return aStar.Terrain.isBomb(type);
    }


    /// <summary>
    /// 传入位置放下炸弹是否有价值
    /// </summary>
    /// <param name="point2d"></param>
    /// <returns></returns>
    public bool isHaveValueForBomb(ref Point2D point2d)
    {
        if (!this.isPointVaild(ref point2d))
            return false;
        byte typeByte = this.mapBytes[point2d.x, point2d.y];
        TerrainType type = (TerrainType)Enum.ToObject(typeof(TerrainType), typeByte);
        if (aStar.Terrain.valueForBomb(type) > 0)
            return true;
        return false;
    }

    /// <summary>
    ///  传入位置是否有被炸到的危险
    /// </summary>
    /// <param name="point2d"></param>
    /// <returns></returns>
    public bool isDangerPosition(ref Point2D point2d)
    {
        if (!this.isPointVaild(ref point2d))
            return false;
        if (this.dangerPerformance.ContainsKey(point2d) && this.dangerPerformance[point2d] > 0)
            return true;

        return false;
    }

    /// <summary>
    /// 当前爆炸范围
    /// </summary>
    /// <returns></returns>
    public Point2D[] getBombRang(int bombPower, ref Point2D point2d)
    {
        Point2D[] rang = this.getRang(bombPower, ref point2d);
        for (int index = 0; index < rang.Length; index++)
        {
            //检查是否超出游戏范围
            if (!this.isBomb(ref rang[index]))
            {
                rang[index].isVaild = false;
            }
        }
        return rang;
    }


    /// <summary>
    /// 简单粗略的计算两点相对位置
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns>p2在p1的什么位置</returns>
    public DirectionType relativePosition(ref Point2D p1, ref  Point2D p2)
    {
        if (p2.x < p1.x && p2.y == p1.y)
        {
            return DirectionType.Left;
        }
        else if (p2.x > p1.x && p2.y == p1.y)
        {
            return DirectionType.Right;
        }
        else if (p2.y < p1.y && p2.x == p1.x)
        {
            return DirectionType.Down;
        }
        else if (p2.y > p1.y && p2.x == p1.x)
        {
            return DirectionType.Up;
        }

        return DirectionType.Origin;
    }


    /// <summary>
    /// 是否为无效的坐标
    /// </summary>
    /// <param name="point2d"></param>
    /// <returns></returns>
    public bool isPointVaild(ref Point2D point2d)
    {
        if (!point2d.isVaild)
            return false;
        if (!this.gameRect.Contains(new Vector2(point2d.x, point2d.y)))
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 通过一个点和范围值，获得点的范围坐标
    /// </summary>
    /// <param name="point2d"></param>
    /// <param name="rangValue"></param>
    /// <returns>返回的是点根据当前方向计算出的范围坐标，下标最后一位代表自己</returns>
    public Point2D[] getRang(int rangValue, ref Point2D point2d, bool isContainSelf = true)
    {
        int directionLen = Direction.MoveDirection.Length;

        Point2D[] rang = new Point2D[isContainSelf ? rangValue * directionLen + 1 : rangValue*directionLen];

        for (int j = 0; j < rangValue; j++)
        {
            for (int i = 0; i < directionLen; i++)
            {
                int index = j * directionLen + i;
                rang[index] = point2d + new Point2D(Direction.MoveDirection[i].x * (j + 1), Direction.MoveDirection[i].y * (j + 1)); //自身的位置+四周
            }
        }

        if (isContainSelf)
            rang[rang.Length - 1] = point2d;
        return rang;
    }


    /// <summary>
    /// 3D 转2D 坐标
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <returns></returns>
    public Point2D point3DTo2D(Transform tran)
    {
        int _x = (int)tran.localPosition.x;
        int _y = (int)tran.localPosition.z;
        if (_x % UnitSize > 0 || _y % UnitSize > 0)
        {
            Debug.LogError("Input value is invalid");
            return new Point2D(0, 0);
        }

        _x = _x / UnitSize;
        _y = _y / UnitSize;

        return new Point2D(_x, _y);
    }


    /// <summary>
    /// 2D转3D 
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public Vector3 point2DTo3D(ref Point2D p)
    {
        return new Vector3(p.x * UnitSize, 0, p.y * UnitSize);
    }

}
