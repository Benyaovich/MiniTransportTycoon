public interface IHasCellModel
{
    public Cell Model { get; }
    public void SetModel(Cell cell);
    public void ClearModel();
}