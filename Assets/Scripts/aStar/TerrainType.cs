namespace aStar
{
    /// <summary>
    /// 地形
    /// </summary>
    public enum TerrainType
    {
        /// <summary>
        /// 空地
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 木质
        /// </summary>
        Wood = 1,

        /// <summary>
        /// 水
        /// </summary>
        Water = 2,

        /// <summary>
        /// 草地
        /// </summary>
        Lawn = 3,


        Actor = 97,

        Bomb = 98,

        /// <summary>
        /// 不可到达
        /// </summary>
        NoWay = 99

    }


    public class Terrain
    {
        /// <summary>
        /// 是否可以到达
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool isArrive(TerrainType type)
        {
            switch (type)
            {
                case TerrainType.NoWay:
                    return false;
                case TerrainType.Water:
                    return false;
                case TerrainType.Wood:
                    return false;
                case TerrainType.Bomb:
                    return false;
                case TerrainType.Actor:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 传入地形是否可以炸
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool isBomb(TerrainType type)
        {
            switch (type)
            {
                case TerrainType.NoWay:
                    return false;
                default:
                    return true;
            }
        }


        /// <summary>
        /// 传入的地形是否有价值去炸
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int valueForBomb(TerrainType type)
        {
            int value = 0;
            switch (type)
            {
//                case TerrainType.Actor:
//                    value = 2;
//                    break;
                case TerrainType.Wood:
                    value = 1;
                    break;
                default:
                    value = 0;
                    break;
            }
            return value;
        }
    }
}


