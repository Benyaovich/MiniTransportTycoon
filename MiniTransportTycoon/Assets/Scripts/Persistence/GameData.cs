using System.Collections.Generic;
using Model.Vehicles.Buses;
using Model.Vehicles.CargoTrucks;
using Model.Vehicles.SemiTrucks;

[System.Serializable]
public class GameData
{
    public List<SModelGridObject> gridArray = new();
    public List<SCity> cities = new();
    public List<SVehicle> vehicles = new();
    public int money;
    public GameData(ModelGridObject[,] gridArray, List<City> cities, List<Vehicle> vehicles,int money)
    {
        this.money = money;
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
                this.vehicles.Add(new SSlowBus(slowBus));
            if (vehicle is FastBus fastBus)
                this.vehicles.Add(new SFastBus(fastBus));
            if (vehicle is CoalCargoTruck coalCargoTruck)
                this.vehicles.Add(new SCoalCargoTruck(coalCargoTruck));
            if (vehicle is IronCargoTruck ironCargoTruck)
                this.vehicles.Add(new SIronCargoTruck(ironCargoTruck));
            if (vehicle is PaperCargoTruck paperCargoTruck)
                this.vehicles.Add(new SPaperCargoTruck(paperCargoTruck));
            if (vehicle is SteelCargoTruck steelCargoTruck)
                this.vehicles.Add(new SSteelCargoTruck(steelCargoTruck));
            if (vehicle is WoodCargoTruck woodCargoTruck)
                this.vehicles.Add(new SWoodCargoTruck(woodCargoTruck));
            if (vehicle is CoalSemiTruck coalSemiTruck)
                this.vehicles.Add(new SCoalSemiTruck(coalSemiTruck));
            if (vehicle is IronSemiTruck ironSemiTruck)
                this.vehicles.Add(new SIronSemiTruck(ironSemiTruck));
            if (vehicle is PaperSemiTruck paperSemiTruck)
                this.vehicles.Add(new SPaperSemiTruck(paperSemiTruck));
            if (vehicle is SteelSemiTruck steelSemiTruck)
                this.vehicles.Add(new SSteelSemiTruck(steelSemiTruck));
            if (vehicle is WoodSemiTruck woodSemiTruck)
                this.vehicles.Add(new SWoodSemiTruck(woodSemiTruck));
        }
    }
    
    public GameData(){}
}
