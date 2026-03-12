public interface IGrid<T>
{
    public Size Size { get; }
    public T GetGridObject(int x, int y);
    public T GetGridObject(Location location);
    public void SetGridObject(int x, int y, T value);
}
