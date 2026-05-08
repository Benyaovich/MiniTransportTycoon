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
                Build(new SmallCity(
                    origin: ToLocation(city.origin),
                    size: ToSize(city.size),
                    numOfPeople: city.numOfPeople
                ));
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
                    Build(new Forest(
                        origin: ToLocation(forest.origin),
                        size: ToSize(forest.size),
                        numOfTrees: forest.numOfTrees
                    ));
                    break;

                case SBusStop busStop:
                    Build(new BusStop(
                        location: ToLocation(busStop.origin),
                        size: ToSize(busStop.size),
                        numOfPeople: busStop.numOfPeople
                    ));
                    break;

                case SExtractorBuildingIron iron:
                    Build(new ExtractorBuildingIron(
                        loc: ToLocation(iron.origin),
                        size: ToSize(iron.size),
                        resourceAmount: iron.resourceAmount
                    ));
                    break;

                case SExtractorBuildingWood wood:
                    Build(new ExtractorBuildingWood(
                        loc: ToLocation(wood.origin),
                        size: ToSize(wood.size),
                        resourceAmount: wood.resourceAmount
                    ));
                    break;

                case SExtractorBuildingCoal coal:
                    Build(new ExtractorBuildingCoal(
                        loc: ToLocation(coal.origin),
                        size: ToSize(coal.size),
                        resourceAmount: coal.resourceAmount
                    ));
                    break;

                case SProcessingBuildingPaper paper:
                    Build(new ProcessingBuildingPaper(
                        loc: ToLocation(paper.origin),
                        size: ToSize(paper.size),
                        requiredResourceAmount: paper.requiredResourceAmount,
                        resourceAmount: paper.resourceAmount
                    ));
                    break;

                case SProcessingBuildingSteel steel:
                    Build(new ProcessingBuildingSteel(
                        loc: ToLocation(steel.origin),
                        size: ToSize(steel.size),
                        requiredResourceAmount: steel.requiredResourceAmount,
                        resourceAmount: steel.resourceAmount
                    ));
                    break;
                
                case SResidentialBuilding1 building:
                    Build(new ResidentialBuilding1(
                        origin: ToLocation(building.origin),
                        size: ToSize(building.size)
                    ));              
                    break;
                
                case SResidentialBuilding2 building:
                    Build(new ResidentialBuilding2(
                        origin: ToLocation(building.origin),
                        size: ToSize(building.size)
                    ));              
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
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new SlowBus(
                            GridManager.Instance.Grid,
                            resourceAmount: slowBus.resourceAmount,
                            route: ToRoute(slowBus.route),
                            maintenanceRemainingTime: slowBus.maintenanceRemainingTime,
                            moveRemainingTime: slowBus.moveRemainingTime
                        )
                    );
                    break;

                case SFastBus fastBus:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new FastBus(
                            GridManager.Instance.Grid,
                            resourceAmount: fastBus.resourceAmount,
                            route: ToRoute(fastBus.route),
                            maintenanceRemainingTime: fastBus.maintenanceRemainingTime,
                            moveRemainingTime: fastBus.moveRemainingTime
                        )
                    );
                    break;

                case SCoalCargoTruck coalCargoTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new CoalCargoTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: coalCargoTruck.resourceAmount,
                            route: ToRoute(coalCargoTruck.route),
                            maintenanceRemainingTime: coalCargoTruck.maintenanceRemainingTime,
                            moveRemainingTime: coalCargoTruck.moveRemainingTime
                        )
                    );
                    break;

                case SIronCargoTruck ironCargoTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new IronCargoTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: ironCargoTruck.resourceAmount,
                            route: ToRoute(ironCargoTruck.route),
                            maintenanceRemainingTime: ironCargoTruck.maintenanceRemainingTime,
                            moveRemainingTime: ironCargoTruck.moveRemainingTime
                        )
                    );
                    break;

                case SPaperCargoTruck paperCargoTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new PaperCargoTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: paperCargoTruck.resourceAmount,
                            route: ToRoute(paperCargoTruck.route),
                            maintenanceRemainingTime: paperCargoTruck.maintenanceRemainingTime,
                            moveRemainingTime: paperCargoTruck.moveRemainingTime
                        )
                    );
                    break;

                case SSteelCargoTruck steelCargoTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new SteelCargoTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: steelCargoTruck.resourceAmount,
                            route: ToRoute(steelCargoTruck.route),
                            maintenanceRemainingTime: steelCargoTruck.maintenanceRemainingTime,
                            moveRemainingTime: steelCargoTruck.moveRemainingTime
                        )
                    );
                    break;

                case SWoodCargoTruck woodCargoTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new WoodCargoTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: woodCargoTruck.resourceAmount,
                            route: ToRoute(woodCargoTruck.route),
                            maintenanceRemainingTime: woodCargoTruck.maintenanceRemainingTime,
                            moveRemainingTime: woodCargoTruck.moveRemainingTime
                        )
                    );
                    break;

                case SCoalSemiTruck coalSemiTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new CoalSemiTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: coalSemiTruck.resourceAmount,
                            route: ToRoute(coalSemiTruck.route),
                            maintenanceRemainingTime: coalSemiTruck.maintenanceRemainingTime,
                            moveRemainingTime: coalSemiTruck.moveRemainingTime
                        )
                    );
                    break;

                case SIronSemiTruck ironSemiTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new IronSemiTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: ironSemiTruck.resourceAmount,
                            route: ToRoute(ironSemiTruck.route),
                            maintenanceRemainingTime: ironSemiTruck.maintenanceRemainingTime,
                            moveRemainingTime: ironSemiTruck.moveRemainingTime
                        )
                    );
                    break;

                case SPaperSemiTruck paperSemiTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new PaperSemiTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: paperSemiTruck.resourceAmount,
                            route: ToRoute(paperSemiTruck.route),
                            maintenanceRemainingTime: paperSemiTruck.maintenanceRemainingTime,
                            moveRemainingTime: paperSemiTruck.moveRemainingTime
                        )
                    );
                    break;

                case SSteelSemiTruck steelSemiTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new SteelSemiTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: steelSemiTruck.resourceAmount,
                            route: ToRoute(steelSemiTruck.route),
                            maintenanceRemainingTime: steelSemiTruck.maintenanceRemainingTime,
                            moveRemainingTime: steelSemiTruck.moveRemainingTime
                        )
                    );
                    break;

                case SWoodSemiTruck woodSemiTruck:
                    VehicleManager.Instance.VehicleStorage.AddVehicle(
                        new WoodSemiTruck(
                            GridManager.Instance.Grid,
                            resourceAmount: woodSemiTruck.resourceAmount,
                            route: ToRoute(woodSemiTruck.route),
                            maintenanceRemainingTime: woodSemiTruck.maintenanceRemainingTime,
                            moveRemainingTime: woodSemiTruck.moveRemainingTime
                        )
                    );
                    break;
            }
        }
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
