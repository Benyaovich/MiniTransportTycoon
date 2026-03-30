using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using UnityEngine;

public class JsonFileManager : IFileManager
{
    public async Task<GameData> LoadAsync(string path)
    {
        try
        {
            using StreamReader reader = new StreamReader(path);
            return JsonUtility.FromJson<GameData>(await reader.ReadToEndAsync());
        }
        catch
        {
            throw new InvalidDataContractException();
        }

    }

    public async Task SaveAsync(string path, GameData gameData)
    {
        try
        {
            await using StreamWriter writer = new StreamWriter(path);
            await writer.WriteLineAsync(JsonUtility.ToJson(gameData,true));
        }
        catch
        {
            throw new InvalidDataContractException();
        }
    }
}
