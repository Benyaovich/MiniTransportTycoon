public class SmallCity : City
{
    public SmallCity(Location origin, Size size = null, bool destroyable = true, RateChangeHandler rch = null, int numOfPeople = 100)
        : base(origin, size, destroyable, rch, numOfPeople)
    {
        Size = size ?? new Size(3, 3);
    }
}
