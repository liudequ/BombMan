using behaviac;

namespace aStar
{
    /// <summary>
    /// 方向及方向所增加的G值
    /// 可通过更改G值，来让寻路优化考虑某个方向
    /// </summary>
    public struct Direction
    {
        public int x;
        public int y;
        public int G;
        public DirectionType dir;

        public Direction(int _x, int _y, int _G, DirectionType _dir)
        {
            this.x = _x;
            this.y = _y;
            this.G = _G;
            this.dir = _dir;
        }


        #region 方向结构  数据下标需与方向枚举值对应

        //    private static Direction[] MoveDirection = new Direction[8]
        //    {
        //        new Direction(0,1,2,DirectionType.Up),new Direction(-1,1,3,DirectionType.LeftTop),
        //        new Direction(-1,0,2,DirectionType.Left),new Direction(-1,-1,3,DirectionType.LeftBottom),    
        //        new Direction(0,-1,2,DirectionType.Down), new Direction(1,-1,3,DirectionType.RightBottom), 
        //        new Direction(1,0,2,DirectionType.Right), new Direction(1,1,3,DirectionType.RightTop)
        //    };

        public static Direction[] MoveDirection = new Direction[4]
        {
            new Direction(-1,0,2,DirectionType.Left),
            new Direction(0,1,2,DirectionType.Up),   
            new Direction(1,0,2,DirectionType.Right),
            new Direction(0,-1,2,DirectionType.Down), 
        };


        public static Direction GetDirenction(DirectionType type)
        {
            switch (type)
            {
                case DirectionType.Down:
                    return MoveDirection[3]; 
                case DirectionType.Left:
                    return MoveDirection[0];
                case DirectionType.Right:
                    return MoveDirection[2];
                case DirectionType.Up:
                    return MoveDirection[1];
                default:
                    return MoveDirection[0];
                    break;
            }
        }

        #endregion
    }









    /// <summary>
    /// 方向枚举
    /// </summary>
    [behaviac.TypeMetaInfo()]
    public enum DirectionType
    {
        Origin = -1,
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        LeftTop = 4,
        LeftBottom = 5,
        RightBottom = 6,
        RightTop = 7,
    }

}


