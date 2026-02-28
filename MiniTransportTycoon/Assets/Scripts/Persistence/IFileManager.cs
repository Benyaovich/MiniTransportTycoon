using System.IO;
using System.Threading.Tasks;

public interface IFileManager
{
    public Task<GameData> LoadAsync(Stream stream);
    public Task SaveAsync(Stream stream, GameData gameData);
}
