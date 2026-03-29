namespace Model.Interfaces
{
    public interface IDepositPoint
    {
        public Resource RequiredResource { get; }
        public int AddResource(int amount);
    }
}