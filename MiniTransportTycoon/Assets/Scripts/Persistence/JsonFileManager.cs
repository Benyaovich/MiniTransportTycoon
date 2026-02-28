using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using UnityEngine;

public class JsonFileManager : IFileManager
{
    public async Task<GameData> LoadAsync(Stream stream)
    {
        try
        {
            using StreamReader reader = new StreamReader(stream);
            return JsonUtility.FromJson<GameData>(await reader.ReadToEndAsync());
        }
        catch
        {
            throw new InvalidDataContractException();
        }

    }

    public async Task SaveAsync(Stream stream, GameData gameData)
    {
        try
        {
            await using StreamWriter writer = new StreamWriter(stream);
            await writer.WriteLineAsync(JsonUtility.ToJson(gameData));
        }
        catch
        {
            throw new InvalidDataContractException();
        }
    }
}
