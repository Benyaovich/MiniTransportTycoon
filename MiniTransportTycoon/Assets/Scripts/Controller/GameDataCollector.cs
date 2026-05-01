using Controller.Vehicles;
using Model;

public class GameDataCollector
{
    public GameData Create()
    {
        var gridArray = GridManager.Instance.Grid.GridArray;
        var cities = GridManager.Instance.CellBuildingManager.CityService.Cities;
        var vehicles = VehicleManager.Instance.VehicleStorage.Vehicles;

        return new GameData(gridArray, cities, vehicles, PlayerState.Instance.Money);
    }
}
