using System.Threading.Tasks;

public interface IFileManager
{
    public GameData Deserialize (string json);
    public string Serialize (GameData gameData);
    public Task SaveAsync(string path, GameData gameData);
}
