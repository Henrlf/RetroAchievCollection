using System.Text.Json.Serialization;

namespace RetroAchievCollection.Models;

public class AchievementModel
{
    [JsonPropertyName("id")]
    public int Id {get; set;}
    
    [JsonPropertyName("name")]
    public string Name {get; set;} = "";
    
    [JsonPropertyName("description")]
    public string Description {get; set;} = "";
    
    [JsonPropertyName("imagePath")]
    public string ImagePath {get; set;} = "";
    
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted {get; set;}
    
    [JsonPropertyName("isCompletedHardcore")]
    public bool IsCompletedHardcore {get; set;}
}