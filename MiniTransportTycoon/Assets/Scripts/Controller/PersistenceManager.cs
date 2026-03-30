using System.Collections;
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
        
        FileBrowser.SetDefaultFilter(".json");
    }
    
    public IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Select File", "Load" );
        
        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
            OnFilesSelected( FileBrowser.Result );
    }

    public void OnFilesSelected(string[] filePaths)
    {
        
        string destinationPath = filePaths[0];
        Debug.Log(destinationPath);
        Debug.Log(GridManager.Instance.Grid.GridArray[0,0].ToString());
        fileManager.SaveAsync(destinationPath,new GameData(GridManager.Instance.Grid.GridArray));
    }

    public void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowLoadDialogCoroutine());
        }
    }
}
