using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
// ez a 3 import kell a WebGl filebrowserhez ne töröld ki
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Controller.Vehicles;
using JetBrains.Annotations;
using Model;
using Model.Cells.Facility;
using Model.Vehicles.Buses;
using Model.Vehicles.CargoTrucks;
using Model.Vehicles.SemiTrucks;

public class PersistenceManager : MonoBehaviour
{
    private IFileManager _fileManager;
    public static PersistenceManager Instance { get; private set; }
    
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string fileName, byte[] byteArray, int byteArraySize);

    public void OnClickOpen()
    {
        UploadFile(gameObject.name, "OnFileUpload", ".json", false);
    }

    public void OnClickSave(){
        GameData gameData = CollectGameData();
        string json = _fileManager.Serialize(gameData);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        DownloadFile(gameObject.name, "OnFileDownload", "gamesave.json", bytes, bytes.Length);  
    }

    public void OnFileDownload() {}

    public void OnFileUpload(string url)
    {
        StartCoroutine(OutputRoutineOpen(url));
    }

#else

    public void OnClickOpen()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);
        
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    public async void OnClickSave()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "gamesave", "json");
        if (!string.IsNullOrEmpty(path))
        {
            GameData gameData = CollectGameData();
            await _fileManager.SaveAsync(path, gameData);
            Debug.Log($"Saved: {path}");
        }
    }

#endif

    private IEnumerator OutputRoutineOpen(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            try
            {
                GameData gameData = _fileManager.Deserialize(www.downloadHandler.text);
                ApplyGameData(gameData);
                Debug.Log("Loaded save file.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load save file: {ex}");
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        _fileManager = new JsonFileManager();
        DontDestroyOnLoad(gameObject);
    }
    
    private GameData CollectGameData()
    {
        var gridArray = GridManager.Instance.Grid.GridArray;
        var cities = GridManager.Instance.CellBuildingManager.CityService.Cities;
        var vehicles = VehicleManager.Instance.VehicleStorage.Vehicles;
        return new GameData(gridArray,cities,vehicles,PlayerState.Instance.Money);
    }

    private void ApplyGameData(GameData gameData)
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
                    destroyable: city.destroyable,
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
                        destroyable: forest.destroyable,
                        numOfTrees: forest.numOfTrees
                    ));
                    break;

                case SBusStop busStop:
                    Build(new BusStop(
                        location: ToLocation(busStop.origin),
                        size: ToSize(busStop.size),
                        destroyable: busStop.destroyable,
                        numOfPeople: busStop.numOfPeople
                    ));
                    break;

                case SExtractorBuildingIron iron:
                    Build(new ExtractorBuildingIron(
                        loc: ToLocation(iron.origin),
                        size: ToSize(iron.size),
                        destroyable: iron.destroyable,
                        resourceAmount: iron.resourceAmount
                    ));
                    break;

                case SExtractorBuildingWood wood:
                    Build(new ExtractorBuildingWood(
                        loc: ToLocation(wood.origin),
                        size: ToSize(wood.size),
                        destroyable: wood.destroyable,
                        resourceAmount: wood.resourceAmount
                    ));
                    break;

                case SExtractorBuildingCoal coal:
                    Build(new ExtractorBuildingCoal(
                        loc: ToLocation(coal.origin),
                        size: ToSize(coal.size),
                        destroyable: coal.destroyable,
                        resourceAmount: coal.resourceAmount
                    ));
                    break;

                case SProcessingBuildingPaper paper:
                    Build(new ProcessingBuildingPaper(
                        loc: ToLocation(paper.origin),
                        size: ToSize(paper.size),
                        destroyable: paper.destroyable,
                        requiredResourceAmount: paper.requiredResourceAmount,
                        resourceAmount: paper.resourceAmount
                    ));
                    break;

                case SProcessingBuildingSteel steel:
                    Build(new ProcessingBuildingSteel(
                        loc: ToLocation(steel.origin),
                        size: ToSize(steel.size),
                        destroyable: steel.destroyable,
                        requiredResourceAmount: steel.requiredResourceAmount,
                        resourceAmount: steel.resourceAmount
                    ));
                    break;

                case SRoadCell road:
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
        GridManager.Instance.CellBuildingManager.TryBuild(building);
    }
}