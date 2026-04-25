using System.Text.Json.Serialization;

namespace RetroAchievCollection.Models.Json;

public class ConfigurationModel
{
    [JsonPropertyName("userName")]
    public string UserName {get; set;} = "";
    
    [JsonPropertyName("apiKey")]
    public string ApiKey {get; set;} = "";
}