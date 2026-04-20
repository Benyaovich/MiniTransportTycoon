using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class JsonFileManager : IFileManager
{
    private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };
    public async Task<GameData> LoadAsync(string path)
    {
        try
        {
            using StreamReader reader = new StreamReader(path);
            return JsonConvert.DeserializeObject<GameData>(await reader.ReadToEndAsync(),_jsonSettings);
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
            string json = JsonConvert.SerializeObject(gameData, _jsonSettings);
            await using StreamWriter writer = new StreamWriter(path);
            await writer.WriteLineAsync(json);
        }
        catch
        {
            throw new InvalidDataContractException();
        }
    }
}
