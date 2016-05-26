namespace aStar
{
    public interface IDebugAStarFind
    {


        void DrawInitMap(int x, int y, byte[,] mapBytes);

        void RestMap();

        void DrawClose(AStarPosition close);

        void DrawOpen(AStarPosition open);

        void DrawPath(AStarPosition path);
    }

}

