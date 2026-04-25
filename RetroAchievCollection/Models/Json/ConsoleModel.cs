using System.Text.Json.Serialization;

namespace RetroAchievCollection.Models.Json;

public class ConsoleModel
{
    [JsonPropertyName("id")]
    public int Id {get; set;}

    [JsonPropertyName("name")]
    public string Name {get; set;} = "";

    [JsonPropertyName("company")]
    public string Company {get; set;} = "";

    [JsonPropertyName("imagePath")]
    public string ImagePath {get; set;} = "";
}