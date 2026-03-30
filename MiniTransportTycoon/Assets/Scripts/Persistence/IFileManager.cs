using System.IO;
using System.Threading.Tasks;

public interface IFileManager
{
    public Task<GameData> LoadAsync(string path);
    public Task SaveAsync(string path, GameData gameData);
}
