
using System;
using System.Numerics;

public class Grid<T> : IGrid<T>
{
    #region Events

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    #endregion

    #region Properties

    public Size Size { get; private set; }
    public float CellSize { get; private set; }
    

    #endregion

    #region Fields
    
    private readonly Vector3 _originPosition;
    private readonly T[,] _gridArray;

    #endregion

    public Grid(Size size, float cellSize, Vector3 originPosition, 
         Func<Grid<T>, Location, T> createGridObject)
    {
        Size = size;
        CellSize = cellSize;
        _originPosition = originPosition;
        _gridArray = new T[size.Width, size.Height];
        
        for (int y = 0; y < Size.Height; y++)
        {
            for (int x = 0; x < Size.Width; x++)
            {
                _gridArray[x, y] = createGridObject(this, new Location(x,y));
            }
        }
    }


    #region GetWorlPosition

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * CellSize + _originPosition;
    }

    public Vector3 GetWorldPosition(Location loc)
    {
        return GetWorldPosition(loc.X, loc.Y);
    }
    

    #endregion

    #region GetCoordinatesXYOrLocation

        public void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = (int)MathF.Floor((worldPosition - _originPosition).X / CellSize);
            y = (int)MathF.Floor((worldPosition - _originPosition).Y / CellSize);
        }
        
        public Location GetXY(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return new Location(x, y);
        }
    

    #endregion
    
    #region SetGridObjects
    public void SetGridObject(int x, int y, T value)
    {
        if (x < 0 || y < 0 || x >= Size.Width || y >= Size.Height) return;
        
        _gridArray[x, y] = value;
        InvokeOnGridObjectChanged(x,y);
    }
    
    public void SetGridObject(Location l, T value) {
        SetGridObject(l.X, l.Y, value);
    }
    
    public void SetGridObject(Vector3 worldPosition, T value) {
        SetGridObject(GetXY(worldPosition), value);
    }
    
    #endregion
    
    #region GetGridObjects
    public T GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < Size.Width && y < Size.Height) {
            return _gridArray[x, y];
        }
        
        return default;
        
    }

    public T GetGridObject(Location location)
    {
        return GetGridObject(location.X, location.Y);
    }
    
    public T GetGridObject(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }
    
    #endregion

    #region InvokeOnGridObjectChangedEvent
    
    public void InvokeOnGridObjectChanged(int x, int y) {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { Location = new Location(x,y) });
    }
    public void InvokeOnGridObjectChanged(Location loc) {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { Location = loc });
    }
    
    #endregion

}

public class OnGridObjectChangedEventArgs : EventArgs
{  
    public Location Location;
}