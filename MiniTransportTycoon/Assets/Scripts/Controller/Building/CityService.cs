
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityService
{
    public Dictionary<Location, City> CityByLocationMap { get; private set; } = new();
    public List<City> Cities { get; private set; } = new();
    

    public void AddCity(Cell cell, List<Location> gridPositionList)
    {
        if (cell is not City city) return;
        
        if(AreaOccupiedByOtherCity(gridPositionList)) return;
        
        Cities.Add(city);
        AddCityLocationsToMap(city, gridPositionList);
    }

    private void AddCityLocationsToMap(City city, List<Location> gridPositionList)
    {
        foreach (Location location in gridPositionList)
        {
            CityByLocationMap.Add(location, city);
        }
    }

    private bool AreaOccupiedByOtherCity(List<Location> gridPositionList)
    {
        return gridPositionList.Any(location => CityByLocationMap.ContainsKey(location));
    }

}
