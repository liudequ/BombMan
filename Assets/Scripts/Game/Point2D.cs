using UnityEngine;
using System.Collections;
using aStar;

public struct Point2D
{

    public Point2D(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public int x;

    public int y;



    public static Point2D Invaild
    {
        get
        {
            return new Point2D(-1, -1);
        }
    }



    public bool isVaild
    {
        set { if (!value) this.x = this.y = -1; }
        get
        {
            if (this.x != -1 && this.y != -1)
                return true;
            return false;
        }
    }


    public static Point2D operator +(Point2D lhs, Point2D rhs)
    {
        return new Point2D(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public static Point2D operator -(Point2D lhs, Point2D rhs)
    {
        return new Point2D(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    public static Point2D operator +(Point2D lhs, Direction dir)
    {
        return new Point2D(lhs.x + dir.x, lhs.y + dir.y);
    }

    public static Point2D operator -(Point2D lhs, Direction dir)
    {
        return new Point2D(lhs.x - dir.x, lhs.y - dir.y);
    }

    public static Point2D operator *(Point2D lhs, int value)
    {
        return new Point2D(lhs.x * value, lhs.y * value);
    }

    public static Point2D operator /(Point2D lhs, int value)
    {
        return new Point2D(lhs.x / value, lhs.y * value);
    }

    public static bool operator ==(Point2D lhs, Point2D rhs)
    {
        return (lhs.x == rhs.x && lhs.y == rhs.y);
    }

    public static bool operator !=(Point2D lhs, Point2D rhs)
    {
        return (lhs.x != rhs.x || lhs.y != rhs.y);
    }

    public override bool Equals(object obj)
    {
        Point2D other = (Point2D)obj;
        if (this.x == other.x && this.y == other.y)
            return true;
        return false;
    }
}
