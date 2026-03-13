public class SmallCity : City
{
    public SmallCity(Location origin, Size size = null, bool destroyable = true, RateChangeHandler rch = null)
        : base(origin, size, destroyable, rch)
    {
        Size = size ?? new Size(3, 3);
    }
}
