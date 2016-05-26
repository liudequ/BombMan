namespace aStar
{
    /// <summary>
    /// A星寻路中坐标结构
    /// </summary>
    public class AStarPosition
    {
        private int _x;

        private int _y;

        private float _G;

        private float _H;

        private float _F;

        private AStarPosition _parent;

        private DirectionType _dir;

        /// <summary>
        /// 当前寻路中x位置,不一定代表真实x位置
        /// </summary>
        public int x
        {
            set { this._x = value; }
            get { return this._x; }
        }

        /// <summary>
        /// 当前寻路中y位置,不一定代表真实y位置
        /// </summary>
        public int y
        {
            set { this._y = value; }
            get { return this._y; }
        }

        /// <summary>
        /// G是从开始点A到达当前方块的移动量。
        /// </summary>
        public float G
        {
            set
            {
                this._G = value;
                this._F = this._G + this._H;
            }
            get { return this._G; }
        }

        /// <summary>
        /// H值是从当前方块到终点的移动量估算值。
        /// </summary>
        public float H
        {
            set
            {
                this._H = value;
                this._F = this._G + this._H;
            }
            get { return this._H; }
        }

        /// <summary>
        /// G+H 路径增量
        /// </summary>
        public float F
        {
            get { return this._F; }
        }


        /// <summary>
        /// 前继（上一个点）
        /// </summary>
        public AStarPosition parent
        {
            set { this._parent = value; }
            get { return this._parent; }
        }

        /// <summary>
        /// 当前点相对于之前点的方向
        /// </summary>
        public DirectionType dir
        {
            set { this._dir = value; }
            get { return this._dir; }
        }

    }


}

