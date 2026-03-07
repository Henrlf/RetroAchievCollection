using System.Text.Json.Serialization;

namespace RetroAchievCollection.RetroAchievements.Dtos;

public class ConsoleDto
{
    [JsonPropertyName("ID")]
    public int ConsoleId {get; set;} = 0;
    
    [JsonPropertyName("Name")]
    public string Name {get; set;} = "";
    
    [JsonPropertyName("userName")]
    public string Company {get; set;} = "";
    
    [JsonPropertyName("IconURL")]
    public string ImageUrl {get; set;} = "";
}