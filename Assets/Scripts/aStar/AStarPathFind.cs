using System;
using System.Collections.Generic;

namespace aStar
{

    /// <summary>
    /// A星寻路
    /// 按照open数组里F值最小的点探索。直到open数组长度为0或者寻路到目的地
    /// 如果走到死胡同，会再从之前探索里找最小F值的点继续探索
    /// 参考资源：http://www.cnblogs.com/zhoug2020/p/3468167.html
    /// </summary>
    public class AStarPathFind
    {


        /// <summary>
        /// 不再考虑的路径点
        /// </summary>
        private List<AStarPosition> _closeArray = new List<AStarPosition>();


        /// <summary>
        /// 存在所有有可能走的路径点
        /// </summary>
        private List<AStarPosition> _openArray = new List<AStarPosition>();


        /// <summary>
        /// 地图X轴最大值
        /// </summary>
        private int mapXSize;

        /// <summary>
        /// 地图Y轴最大值
        /// </summary>
        private int mapYSize;







        private byte[,] mapBytes;

        //todo 重构单例
        public static AStarPathFind instance;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeX">地图X最大值</param>
        /// <param name="sizeY">地图Y最大值</param>
        public AStarPathFind(int sizeX, int sizeY, byte[,] map)
        {
            this.mapXSize = sizeX;
            this.mapYSize = sizeY;
            this.mapBytes = map;
            instance = this;
        }


#if DrawDebug

    private IDebugAStarFind IDebug;

    private float stepSpeed = 0.5f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sizeX">地图X最大值</param>
    /// <param name="sizeY">地图Y最大值</param>
    public AStarPathFind(int sizeX, int sizeY, byte[,] map, IDebugAStarFind debug)
    {
        this.mapXSize = sizeX;
        this.mapYSize = sizeY;
        this.mapBytes = map;

        this.IDebug = debug;
        this.IDebug.DrawInitMap(this.mapXSize,this.mapYSize,this.mapBytes);
        instance = this;
    }
#endif

        /// <summary>
        /// 寻路
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns>为空时代表无法到达</returns>
#if DrawStepDebug 
    public IEnumerator findPath(int fromX, int fromY, int toX, int toY)
#else
        public List<AStarPosition> findPath(int fromX, int fromY, int toX, int toY)
#endif
        {
            this._closeArray.Clear();
            this._openArray.Clear();

            List<AStarPosition> path = null;

            AStarPosition currentPos = new AStarPosition();

            AStarPosition toPos = new AStarPosition();
            toPos.x = toX;
            toPos.y = toY;

            currentPos.x = fromX;
            currentPos.y = fromY;
            currentPos.G = 0;
            currentPos.dir = DirectionType.Origin;

            this._openArray.Add(currentPos);

#if DrawDebug

        this.IDebug.DrawOpen(currentPos);

#endif

            AStarPosition destPos = null;
            do
            {
                int lowIndex = -1;
                currentPos = this.getNextLowF(out lowIndex);
                this._closeArray.Add(currentPos);

#if DrawDebug

            this.IDebug.DrawClose(currentPos);

#endif


                this._openArray.RemoveAt(lowIndex);

#if DrawStepDebug
            yield return new WaitForSeconds(this.stepSpeed);
#endif

                if (currentPos.x == toPos.x && currentPos.y == toPos.y)
                {
                    //find 
                    destPos = currentPos;
                    break;
                }

                List<AStarPosition> adjacentPositions = this.getAdjacentPositions(currentPos, toPos);

                if (adjacentPositions != null)
                {
                    for (int i = adjacentPositions.Count - 1; i >= 0; i--) //根据MoveDirection里设定的方向考虑顺序完成
                    {
                        AStarPosition adjacent = adjacentPositions[i];
                        AStarPosition old = this.findSamePostion(this._closeArray, adjacent);
                        if (old != null) //该点存在于不考虑的范围内
                            continue;
                        old = this.findSamePostion(this._openArray, adjacent);


                        if (old == null)
                        {
                            this._openArray.Add(adjacent);//把所有可能情况加入到考虑范围内
                        }
                        else if (adjacent.F < old.F)//多条探索，经过同一个点
                        {
                            old.G = adjacent.G;
                            old.parent = adjacent.parent;
                            old.dir = adjacent.dir;
                        }

#if DrawDebug
                    this.IDebug.DrawOpen(adjacentPositions[i]);
#endif
                    }
                }
#if DrawStepDebug
            yield return new WaitForSeconds(this.stepSpeed);
#endif
            } while (this._openArray.Count > 0);


            if (destPos != null) // 从初始位置开始的路径信息
            {
                path = new List<AStarPosition>();
                AStarPosition pos = destPos;
                while (pos != null)
                {
                    path.Insert(0, pos);

#if DrawDebug
                this.IDebug.DrawPath(pos);
#endif

                    pos = pos.parent;
                }
            }
            else //无法到达目的地时，如果找到最近可达到点
            {

            }
#if !DrawStepDebug
            return path;
#endif
        }


        /// <summary>
        /// “曼哈顿距离方法”，它只是计算出距离点B，剩下的水平和垂直的方块数量，略去了障碍物或者不同陆地类型的数量。
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        private int getH(AStarPosition fromPos, AStarPosition toPos)
        {
            return (int)Math.Sqrt((fromPos.x - toPos.x) * (fromPos.x - toPos.x) + (fromPos.y - toPos.y) * (fromPos.y - toPos.y));
        }

        /// <summary>
        /// 获取下一个F值最小的可考虑的位置
        /// </summary>
        /// <returns></returns>
        private AStarPosition getNextLowF(out int index)
        {
            AStarPosition nextPos = null;
            float f = int.MaxValue;
            index = -1;
            for (int i = 0; i < this._openArray.Count; i++)
            {
                if (this._openArray[i].F <= f)
                {
                    nextPos = this._openArray[i];
                    f = this._openArray[i].F;
                    index = i;
                }
            }
            return nextPos;
        }

        /// <summary>
        /// 找到与Pos所代表的位置相同的位置
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private AStarPosition findSamePostion(List<AStarPosition> list, AStarPosition pos)
        {
            AStarPosition old = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].x == pos.x && list[i].y == pos.y)
                {
                    old = list[i];
                    break;
                }
            }
            return old;
        }

        /// <summary>
        /// 获取原点相邻所有可移动的位置
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private List<AStarPosition> getAdjacentPositions(AStarPosition origin, AStarPosition toPos)
        {
            List<AStarPosition> positions = new List<AStarPosition>();

            AStarPosition adjacent = null;
            for (int i = 0; i < Direction.MoveDirection.Length; i++)
            {
                adjacent = createAdjacent(Direction.MoveDirection[i], origin, toPos);
                if (adjacent != null)
                {
                    positions.Add(adjacent);
                }
            }

            return positions;
        }

        /// <summary>
        /// 创建相邻的路径点
        /// 需要判断相邻点是否可到达
        /// 且不可超出地图范围
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="origin"></param>
        /// <param name="toPos"></param>
        /// <returns></returns>
        private AStarPosition createAdjacent(Direction direction, AStarPosition origin, AStarPosition toPos)
        {
            AStarPosition adjacent = new AStarPosition();
            adjacent.x = origin.x + direction.x;
            adjacent.y = origin.y + direction.y;


            if (adjacent.x < 0 || adjacent.y < 0 || adjacent.x >= this.mapXSize || adjacent.y >= this.mapYSize)
                return null;

//            if (this.mapBytes[adjacent.x, adjacent.y] == (byte)TerrainType.NoWay)
//                return null;
            byte type = this.mapBytes[adjacent.x, adjacent.y];
            TerrainType terrain = (TerrainType)Enum.Parse(typeof (TerrainType), type.ToString());
            if (!Terrain.isArrive(terrain))
                return null;

            adjacent.G = origin.G + direction.G;
            adjacent.dir = direction.dir;

            adjacent.H = this.getH(adjacent, toPos);

            //距离终点越近的点，H值相对越小，减少冗余位置的计算
            adjacent.H *= 1.5f;

            //修正寻路方向，使最终找到的路径尽可能少拐弯
            if (adjacent.dir != origin.dir)
            {
                adjacent.G += 0.5f;
            }


            adjacent.parent = origin;


            return adjacent;
        }

    }
}

