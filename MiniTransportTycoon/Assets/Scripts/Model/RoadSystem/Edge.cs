using System;

public class Edge
{
    public Location A { get; private set; }
    public Location B { get; private set; }
    public int W { get; private set; } // weight - hossz

    public Edge(Location a, Location b)
    {
        A = a;
        B = b;

        if (A.X == B.X)
        {
            if (A.Y == B.Y)
            {
                throw new ArgumentException("A két csúcs ugyanaz, így nem lehet út");
            }

            if (A.Y > B.Y)
            {
                W = A.Y -  B.Y;
            }
            else
            {
                W = B.Y - A.Y;
            }
        } else if (A.Y == B.Y)
        {
            if (A.X > B.X)
            {
                W = A.X - B.X;
            }
            else
            {
                W = B.X - A.X;
            }
        }
        else
        {
            throw new ArgumentException("A ket csúcs nem egy vonalban van, így nem lehet út");
        }
    }
    
    public static bool operator ==(Edge a, Edge b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return (a.A == b.A && a.B == b.B) || (a.A == b.B && a.B == b.A);
    }

    public static bool operator !=(Edge a, Edge b) => !(a == b);

    public override bool Equals(object obj)
    {
        if (obj is Edge edge) return edge == this;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(A, B, W);
}
