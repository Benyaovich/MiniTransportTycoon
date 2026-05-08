using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Model;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

public class PersistenceManager : MonoBehaviour
{
    private IFileManager _fileManager;
    private GameDataCollector _gameDataCollector;
    private GameDataApplier _gameDataApplier;

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

    public void OnClickSave()
    {
        GameData gameData = _gameDataCollector.Create();
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
            GameData gameData = _gameDataCollector.Create();
            await _fileManager.SaveAsync(path, gameData);
            Debug.Log($"Saved: {path}");
        }
    }

#endif

    private void Awake()
    {
        Instance = this;
        _fileManager = new JsonFileManager();
        _gameDataCollector = new GameDataCollector();
        _gameDataApplier = new GameDataApplier();
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator OutputRoutineOpen(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }

        try
        {
            PlayerState.Instance.SetIsMapLoadingFromPersistence(true);
            GameData gameData = _fileManager.Deserialize(www.downloadHandler.text);
            _gameDataApplier.Apply(gameData);
            PlayerState.Instance.SetIsMapLoadingFromPersistence(false);
            Debug.Log("Loaded save file.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load save file: {ex}");
        }
    }
    
    public void LoadDefaultNewGame()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "basic_map.json");
        string uri = new Uri(path).AbsoluteUri;
        StartCoroutine(OutputRoutineOpen(uri));
    }
}
