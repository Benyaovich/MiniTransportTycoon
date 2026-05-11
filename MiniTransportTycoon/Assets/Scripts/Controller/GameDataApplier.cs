using System.Collections.Generic;
using Controller.Grid;
using Controller.Vehicles;
using JetBrains.Annotations;
using Model;
using Model.Cells.Cities.Houses;
using Model.Cells.Facility;
using Model.Vehicles.Buses;
using Model.Vehicles.CargoTrucks;
using Model.Vehicles.SemiTrucks;

public class GameDataApplier
{
    public void Apply(GameData gameData)
    {
        ApplyCities(gameData.cities);
        ApplyGridItems(gameData.gridArray);
        ApplyVehicles(gameData.vehicles);
        PlayerState.Instance.SetMoney(gameData.money);
    }

    private void ApplyCities(IEnumerable<SCity> cities)
    {
        foreach (var item in cities)
        {
            if (item is SSmallCity city)
            {
                SmallCity loadedCity = new SmallCity(
                    origin: ToLocation(city.origin),
                    size: ToSize(city.size),
                    numOfPeople: city.numOfPeople
                );
                loadedCity.SetRotation(city.rotationDegrees);
                Build(loadedCity);
            }
        }
    }

    private void ApplyGridItems(IEnumerable<SModelGridObject> gridArray)
    {
        foreach (var item in gridArray)
        {
            switch (item.model)
            {
                case SForest forest:
                    Forest loadedForest = new Forest(
                        origin: ToLocation(forest.origin),
                        size: ToSize(forest.size),
                        numOfTrees: forest.numOfTrees
                    );
                    loadedForest.SetRotation(forest.rotationDegrees);
                    Build(loadedForest);
                    break;

                case SBusStop busStop:
                    BusStop loadedBusStop = new BusStop(
                        location: ToLocation(busStop.origin),
                        size: ToSize(busStop.size),
                        numOfPeople: busStop.numOfPeople
                    );
                    loadedBusStop.SetRotation(busStop.rotationDegrees);
                    Build(loadedBusStop);
                    break;

                case SExtractorBuildingIron iron:
                    ExtractorBuildingIron loadedIron = new ExtractorBuildingIron(
                        loc: ToLocation(iron.origin),
                        size: ToSize(iron.size),
                        resourceAmount: iron.resourceAmount
                    );
                    loadedIron.SetRotation(iron.rotationDegrees);
                    Build(loadedIron);
                    break;

                case SExtractorBuildingWood wood:
                    ExtractorBuildingWood loadedWood = new ExtractorBuildingWood(
                        loc: ToLocation(wood.origin),
                        size: ToSize(wood.size),
                        resourceAmount: wood.resourceAmount
                    );
                    loadedWood.SetRotation(wood.rotationDegrees);
                    Build(loadedWood);
                    break;

                case SExtractorBuildingCoal coal:
                    ExtractorBuildingCoal loadedCoal = new ExtractorBuildingCoal(
                        loc: ToLocation(coal.origin),
                        size: ToSize(coal.size),
                        resourceAmount: coal.resourceAmount
                    );
                    loadedCoal.SetRotation(coal.rotationDegrees);
                    Build(loadedCoal);
                    break;

                case SProcessingBuildingPaper paper:
                    ProcessingBuildingPaper loadedPaper = new ProcessingBuildingPaper(
                        loc: ToLocation(paper.origin),
                        size: ToSize(paper.size),
                        requiredResourceAmount: paper.requiredResourceAmount,
                        resourceAmount: paper.resourceAmount
                    );
                    loadedPaper.SetRotation(paper.rotationDegrees);
                    Build(loadedPaper);
                    break;

                case SProcessingBuildingSteel steel:
                    ProcessingBuildingSteel loadedSteel = new ProcessingBuildingSteel(
                        loc: ToLocation(steel.origin),
                        size: ToSize(steel.size),
                        requiredResourceAmount: steel.requiredResourceAmount,
                        resourceAmount: steel.resourceAmount
                    );
                    loadedSteel.SetRotation(steel.rotationDegrees);
                    Build(loadedSteel);
                    break;
                
                case SResidentialBuilding1 building:
                    ResidentialBuilding1 loadedResidential1 = new ResidentialBuilding1(
                        origin: ToLocation(building.origin),
                        size: ToSize(building.size)
                    );
                    loadedResidential1.SetRotation(building.rotationDegrees);
                    Build(loadedResidential1);
                    break;
                
                case SResidentialBuilding2 building:
                    ResidentialBuilding2 loadedResidential2 = new ResidentialBuilding2(
                        origin: ToLocation(building.origin),
                        size: ToSize(building.size)
                    );
                    loadedResidential2.SetRotation(building.rotationDegrees);
                    Build(loadedResidential2);
                    break;

                case SRoadCell road:
                    if (GridManager.Instance != null)
                        GridManager.Instance.DynamicRoadBuildingManager
                            .TryBuildRoad(ToLocation(road.origin));
                    break;
            }
        }
    }

    private void ApplyVehicles(IEnumerable<SVehicle> vehicles)
    {
        foreach (var item in vehicles)
        {
            switch (item)
            {
                case SSlowBus slowBus:
                    SlowBus loadedSlowBus = new SlowBus(
                        GridManager.Instance.Grid,
                        resourceAmount: slowBus.resourceAmount,
                        route: ToRoute(slowBus.route),
                        maintenanceRemainingTime: slowBus.maintenanceRemainingTime,
                        moveRemainingTime: slowBus.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedSlowBus, slowBus);
                    break;

                case SFastBus fastBus:
                    FastBus loadedFastBus = new FastBus(
                        GridManager.Instance.Grid,
                        resourceAmount: fastBus.resourceAmount,
                        route: ToRoute(fastBus.route),
                        maintenanceRemainingTime: fastBus.maintenanceRemainingTime,
                        moveRemainingTime: fastBus.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedFastBus, fastBus);
                    break;

                case SCoalCargoTruck coalCargoTruck:
                    CoalCargoTruck loadedCoalCargoTruck = new CoalCargoTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: coalCargoTruck.resourceAmount,
                        route: ToRoute(coalCargoTruck.route),
                        maintenanceRemainingTime: coalCargoTruck.maintenanceRemainingTime,
                        moveRemainingTime: coalCargoTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedCoalCargoTruck, coalCargoTruck);
                    break;

                case SIronCargoTruck ironCargoTruck:
                    IronCargoTruck loadedIronCargoTruck = new IronCargoTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: ironCargoTruck.resourceAmount,
                        route: ToRoute(ironCargoTruck.route),
                        maintenanceRemainingTime: ironCargoTruck.maintenanceRemainingTime,
                        moveRemainingTime: ironCargoTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedIronCargoTruck, ironCargoTruck);
                    break;

                case SPaperCargoTruck paperCargoTruck:
                    PaperCargoTruck loadedPaperCargoTruck = new PaperCargoTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: paperCargoTruck.resourceAmount,
                        route: ToRoute(paperCargoTruck.route),
                        maintenanceRemainingTime: paperCargoTruck.maintenanceRemainingTime,
                        moveRemainingTime: paperCargoTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedPaperCargoTruck, paperCargoTruck);
                    break;

                case SSteelCargoTruck steelCargoTruck:
                    SteelCargoTruck loadedSteelCargoTruck = new SteelCargoTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: steelCargoTruck.resourceAmount,
                        route: ToRoute(steelCargoTruck.route),
                        maintenanceRemainingTime: steelCargoTruck.maintenanceRemainingTime,
                        moveRemainingTime: steelCargoTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedSteelCargoTruck, steelCargoTruck);
                    break;

                case SWoodCargoTruck woodCargoTruck:
                    WoodCargoTruck loadedWoodCargoTruck = new WoodCargoTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: woodCargoTruck.resourceAmount,
                        route: ToRoute(woodCargoTruck.route),
                        maintenanceRemainingTime: woodCargoTruck.maintenanceRemainingTime,
                        moveRemainingTime: woodCargoTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedWoodCargoTruck, woodCargoTruck);
                    break;

                case SCoalSemiTruck coalSemiTruck:
                    CoalSemiTruck loadedCoalSemiTruck = new CoalSemiTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: coalSemiTruck.resourceAmount,
                        route: ToRoute(coalSemiTruck.route),
                        maintenanceRemainingTime: coalSemiTruck.maintenanceRemainingTime,
                        moveRemainingTime: coalSemiTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedCoalSemiTruck, coalSemiTruck);
                    break;

                case SIronSemiTruck ironSemiTruck:
                    IronSemiTruck loadedIronSemiTruck = new IronSemiTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: ironSemiTruck.resourceAmount,
                        route: ToRoute(ironSemiTruck.route),
                        maintenanceRemainingTime: ironSemiTruck.maintenanceRemainingTime,
                        moveRemainingTime: ironSemiTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedIronSemiTruck, ironSemiTruck);
                    break;

                case SPaperSemiTruck paperSemiTruck:
                    PaperSemiTruck loadedPaperSemiTruck = new PaperSemiTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: paperSemiTruck.resourceAmount,
                        route: ToRoute(paperSemiTruck.route),
                        maintenanceRemainingTime: paperSemiTruck.maintenanceRemainingTime,
                        moveRemainingTime: paperSemiTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedPaperSemiTruck, paperSemiTruck);
                    break;

                case SSteelSemiTruck steelSemiTruck:
                    SteelSemiTruck loadedSteelSemiTruck = new SteelSemiTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: steelSemiTruck.resourceAmount,
                        route: ToRoute(steelSemiTruck.route),
                        maintenanceRemainingTime: steelSemiTruck.maintenanceRemainingTime,
                        moveRemainingTime: steelSemiTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedSteelSemiTruck, steelSemiTruck);
                    break;

                case SWoodSemiTruck woodSemiTruck:
                    WoodSemiTruck loadedWoodSemiTruck = new WoodSemiTruck(
                        GridManager.Instance.Grid,
                        resourceAmount: woodSemiTruck.resourceAmount,
                        route: ToRoute(woodSemiTruck.route),
                        maintenanceRemainingTime: woodSemiTruck.maintenanceRemainingTime,
                        moveRemainingTime: woodSemiTruck.moveRemainingTime
                    );
                    FinalizeLoadedVehicle(loadedWoodSemiTruck, woodSemiTruck);
                    break;
            }
        }
    }

    private static void FinalizeLoadedVehicle(Vehicle vehicle, SVehicle serializedVehicle)
    {
        vehicle.RestoreIdentifier(serializedVehicle.identifier);
        vehicle.SetCityService(VehicleManager.Instance.CityService);
        VehicleManager.Instance.VehicleStorage.AddVehicle(vehicle);
    }

    private static Location ToLocation(SLocation location)
    {
        return new Location(location.x, location.y);
    }

    private static Size ToSize(SSize size)
    {
        return new Size(size.width, size.height);
    }

    private static Queue<Location> ToVerticesQueue(IEnumerable<SLocation> vertices)
    {
        Queue<Location> queue = new();

        foreach (var vertex in vertices)
        {
            queue.Enqueue(ToLocation(vertex));
        }

        return queue;
    }

    [CanBeNull]
    private static Route ToRoute(SRoute route)
    {
        if (route is null) return null;

        return new Route(
            ToVerticesQueue(route.vertices),
            RouteCreationManager.Instance.PathHandler,
            ToLocation(route.previousVertex),
            ToLocation(route.currentVertex),
            ToLocation(route.nextVertex),
            ToLocation(route.previousPosition),
            ToLocation(route.currentPosition),
            ToLocation(route.nextPosition),
            route.isTurning,
            route.currentlyStuck,
            route.turns180Happened,
            route.turns180Finished
        );
    }

    private static void Build(Cell building)
    {
        if(GridManager.Instance && GridManager.Instance.CellBuildingManager != null)
        {
            GridManager.Instance.CellBuildingManager.TryBuild(building);
        }
    }
}
