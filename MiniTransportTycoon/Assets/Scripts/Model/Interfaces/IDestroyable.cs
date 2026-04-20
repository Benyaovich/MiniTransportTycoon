namespace Model.Interfaces
{
    public interface IDestroyable
    {
        public bool CanDestroy { get; }
        public int DestroyPrice { get; }
    }
}