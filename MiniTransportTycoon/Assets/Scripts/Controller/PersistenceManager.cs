using System.Collections;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
// ez a 3 import kell a WebGl filebrowserhez ne töröld ki
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class PersistenceManager : MonoBehaviour
{
    private IFileManager _fileManager;
    private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };
    
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
        string json = JsonConvert.SerializeObject(gameData, _jsonSettings);
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
            Debug.Log(www.downloadHandler.text);
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
        return new GameData();
    }
}