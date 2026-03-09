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

    public override bool Equals(object obj)
    {
        if (obj is Edge e)
        {
            return (A.Equals(e.A) && B.Equals(e.B)) || (A.Equals(e.B) && B.Equals(e.A)) ;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(A, B, W);
    }
}
