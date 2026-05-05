#nullable enable
namespace Model
{
    public class GameEconomy
    {
        private static GameEconomy? _instance;

        public static GameEconomy Instance
        {
            get
            {
                _instance ??= new GameEconomy();
                return _instance;
            }
        }

        public int GetResourcePrice(Resource resource)
        {
            switch (resource)
            {
                case Resource.Iron or Resource.Coal or Resource.Wood:
                    return 10;
                case Resource.People:
                    return 13;
                case Resource.Steel or Resource.Paper:
                    return 15;
            }

            return 0;
        }
    }
}