using System.Text.Json.Serialization;

namespace RetroAchievCollection.Models;

public class ConsoleModel
{
    [JsonPropertyName("id")]
    public int Id {get; set;} = 0;
    
    [JsonPropertyName("name")]
    public string Name {get; set;} = "";
    
    [JsonPropertyName("company")]
    public string Company {get; set;} = "";
    
    [JsonPropertyName("imagePath")]
    public string ImagePath {get; set;} = "";
}