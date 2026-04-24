using System.Collections.Generic;
using Model.Vehicles.Buses;

[System.Serializable]
public class GameData
{
    public List<SModelGridObject> gridArray = new();
    public List<SCity> cities = new();
    public List<SVehicle> vehicles = new();
    public GameData(ModelGridObject[,] gridArray, List<City> cities, List<Vehicle> vehicles)
    {
        foreach (City city in cities)
        {
            if (city is SmallCity smallCity)
            {
                this.cities.Add(new SSmallCity(smallCity));
            }
        }
        
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                if (gridArray[x, y].Model != null)
                {
                    this.gridArray.Add(new SModelGridObject(gridArray[x, y]));
                }
            }
        }

        foreach(Vehicle vehicle in vehicles)
        {
            if (vehicle is SlowBus slowBus)
            {
                this.vehicles.Add(new SSlowBus(slowBus));
            }
        }
    }
    
    public GameData(){}
}
