using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.InputSystem;

public class PersistenceManager : MonoBehaviour
{

    private IFileManager fileManager;
    public static PersistenceManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        fileManager = new JsonFileManager();
        
        FileBrowser.SetFilters(false, new FileBrowser.Filter("JSON files", ".json"));
        FileBrowser.SetDefaultFilter(".json");
    }
    
    public  IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Select File", "Load" );

        if (FileBrowser.Success)
        {
            _ = OnLoadAsync( FileBrowser.Result );
        }
    }

    public IEnumerator ShowSaveDialogCoroutine()
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files);


        if (FileBrowser.Success)
        {
            _ = OnSaveAsync(FileBrowser.Result);
        }
    }

    public async Task OnLoadAsync(string[] filePaths)
    {
        string file = filePaths[0];
        try
        {
            GameData loadedData = await fileManager.LoadAsync(file);
            Debug.Log("hahaoi epiteni kene");
            GridManager.Instance.BuildOnLoad(loadedData.GridArray);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }
    public async Task OnSaveAsync(string[] filePaths)
    {
        string destinationPath = filePaths[0];
        // üres gridarray
        // először majd a várost kell megépíteni
        await fileManager.SaveAsync(destinationPath,new GameData(GridManager.Instance.Grid.GridArray));
        
    }

    public void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowLoadDialogCoroutine());
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowSaveDialogCoroutine());
        }
    }
}
