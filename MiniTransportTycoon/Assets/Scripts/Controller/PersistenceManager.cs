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
using Model.Cells.Facility;
using Model.Vehicles.Buses;

public class PersistenceManager : MonoBehaviour
{
    private IFileManager _fileManager;
    
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

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            OnClickOpen();
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            OnClickSave();
        }
    }

    private void Awake()
    {
        _fileManager = new JsonFileManager();
    }
    
    private GameData CollectGameData()
    {
        var gridArray = GridManager.Instance.Grid.GridArray;
        var cities = GridManager.Instance.CellBuildingManager.CityService.Cities;
        var vehicles = VehicleManager.Instance.VehicleStorage.Vehicles;
        return new GameData(gridArray,cities,vehicles);
    }

    private void ApplyGameData(GameData gameData)
    {
        foreach (var item in gameData.cities)
        {
            if (item is SSmallCity sSmallCity)
            {
                SmallCity smallCity = new SmallCity(
                    origin: new Location(sSmallCity.origin.x, sSmallCity.origin.y),
                    size: new Size(sSmallCity.size.width,sSmallCity.size.height),
                    destroyable: sSmallCity.destroyable,
                    numOfPeople: sSmallCity.numOfPeople
                );
                GridManager.Instance.CellBuildingManager.TryBuild(smallCity);
            }
        }

        foreach (var item in gameData.gridArray)
        {
            if (item.model is SForest sForest)
            {
                Forest forest = new Forest(
                    origin: new Location(sForest.origin.x, sForest.origin.y),
                    size: new Size(sForest.size.width, sForest.size.height),
                    destroyable: sForest.destroyable,
                    numOfTrees: sForest.numOfTrees
                );
                GridManager.Instance.CellBuildingManager.TryBuild(forest);
            }

            if (item.model is SBusStop sBusStop)
            {
                BusStop busStop = new BusStop(
                    location: new Location(sBusStop.origin.x,sBusStop.origin.y),
                    size: new Size(sBusStop.size.width,sBusStop.size.height),
                    destroyable: sBusStop.destroyable,
                    numOfPeople: sBusStop.numOfPeople
                );
                GridManager.Instance.CellBuildingManager.TryBuild(busStop);
            }

            if (item.model is SExtractorBuildingIron sExtractorBuildingIron)
            {
                ExtractorBuildingIron extractorBuildingIron = new ExtractorBuildingIron(
                    loc: new Location(sExtractorBuildingIron.origin.x,sExtractorBuildingIron.origin.y),
                    size: new Size(sExtractorBuildingIron.size.width,sExtractorBuildingIron.size.height),
                    destroyable: sExtractorBuildingIron.destroyable,
                    resourceAmount: sExtractorBuildingIron.resourceAmount
                );
                GridManager.Instance.CellBuildingManager.TryBuild(extractorBuildingIron);
            }
            if (item.model is SExtractorBuildingWood sExtractorBuildingWood)
            {
                ExtractorBuildingWood extractorBuildingWood = new ExtractorBuildingWood(
                    loc: new Location(sExtractorBuildingWood.origin.x,sExtractorBuildingWood.origin.y),
                    size: new Size(sExtractorBuildingWood.size.width,sExtractorBuildingWood.size.height),
                    destroyable: sExtractorBuildingWood.destroyable,
                    resourceAmount: sExtractorBuildingWood.resourceAmount
                );
                GridManager.Instance.CellBuildingManager.TryBuild(extractorBuildingWood);
            }
            if (item.model is SExtractorBuildingCoal sExtractorBuildingCoal)
            {
                ExtractorBuildingCoal extractorBuildingCoal = new ExtractorBuildingCoal(
                    loc: new Location(sExtractorBuildingCoal.origin.x,sExtractorBuildingCoal.origin.y),
                    size: new Size(sExtractorBuildingCoal.size.width,sExtractorBuildingCoal.size.height),
                    destroyable: sExtractorBuildingCoal.destroyable,
                    resourceAmount: sExtractorBuildingCoal.resourceAmount
                );
                GridManager.Instance.CellBuildingManager.TryBuild(extractorBuildingCoal);
            }

            if (item.model is SProcessingBuildingPaper sProcessingBuildingPaper)
            {
                ProcessingBuildingPaper processingBuildingPaper = new ProcessingBuildingPaper(
                    loc: new Location(sProcessingBuildingPaper.origin.x,sProcessingBuildingPaper.origin.y),
                    size: new Size(sProcessingBuildingPaper.size.width,sProcessingBuildingPaper.size.height),
                    destroyable: sProcessingBuildingPaper.destroyable,
                    requiredResourceAmount: sProcessingBuildingPaper.requiredResourceAmount,
                    resourceAmount: sProcessingBuildingPaper.resourceAmount
                );
                GridManager.Instance.CellBuildingManager.TryBuild(processingBuildingPaper);
            }

            if (item.model is SProcessingBuildingSteel sProcessingBuildingSteel)
            {
                ProcessingBuildingSteel processingBuildingSteel = new ProcessingBuildingSteel(
                    loc: new Location(sProcessingBuildingSteel.origin.x,sProcessingBuildingSteel.origin.y),
                    size: new Size(sProcessingBuildingSteel.size.width,sProcessingBuildingSteel.size.height),
                    destroyable: sProcessingBuildingSteel.destroyable,
                    requiredResourceAmount: sProcessingBuildingSteel.requiredResourceAmount,
                    resourceAmount: sProcessingBuildingSteel.resourceAmount
                );
                GridManager.Instance.CellBuildingManager.TryBuild(processingBuildingSteel);
            }

            if (item.model is SRoadCell sRoadCell)
            {
                GridManager.Instance.DynamicRoadBuildingManager.TryBuildRoad(new Location(sRoadCell.origin.x,sRoadCell.origin.y));
            }
        }

        foreach (var item in gameData.vehicles)
        {
            if (item is SSlowBus sSlowBus)
            {
                Queue<Location> vertices = new();
                foreach (var e in sSlowBus.route.vertices)
                {
                    vertices.Enqueue(new Location(e.x, e.y));
                }

                var route = sSlowBus.route;
                SlowBus slowBus = new SlowBus(
                    GridManager.Instance.Grid,
                    resourceAmount: sSlowBus.resourceAmount,
                    route: new Route(
                        vertices,
                        RouteCreationManager.Instance.PathHandler,
                        new Location(route.previousVertex.x, route.previousVertex.y),
                        new Location(route.currentVertex.x, route.currentVertex.y),
                        new Location(route.nextVertex.x, route.nextVertex.y),
                        new Location(route.previousPosition.x, route.previousPosition.y),
                        new Location(route.currentPosition.x, route.currentPosition.y),
                        new Location(route.nextPosition.x, route.nextPosition.y),
                        route.isTurning,
                        route.currentlyStuck,
                        route.turns180Happened,
                        route.turns180Finished
                        ),
                    maintenanceRemainingTime: sSlowBus.maintenanceRemainingTime,
                    moveRemainingTime: sSlowBus.moveRemainingTime
                );
                VehicleManager.Instance.VehicleStorage.AddVehicle(slowBus);
            }
        }
    }
}