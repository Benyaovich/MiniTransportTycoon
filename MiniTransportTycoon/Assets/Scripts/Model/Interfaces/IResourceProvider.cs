namespace Model.Interfaces
{
    public interface IResourceProvider
    {
        public Resource ProducedResource { get; }
        public int GetResource(int amount);
    }
}