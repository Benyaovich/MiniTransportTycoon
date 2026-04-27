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
    public GameData Deserialize(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<GameData>(json,_jsonSettings);
        }
        catch
        {
            throw new InvalidDataContractException();
        }
    }

    public string Serialize(GameData gameData)
    {
        try
        {
            return JsonConvert.SerializeObject(gameData,_jsonSettings);
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
            string json = Serialize(gameData);
            await using StreamWriter writer = new StreamWriter(path);
            await writer.WriteLineAsync(json);
        }
        catch
        {
            throw new InvalidDataContractException();
        }
    }
}
