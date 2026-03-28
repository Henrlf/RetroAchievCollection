using System.Text.Json.Serialization;

namespace RetroAchievCollection.RetroAchievements.Dtos;

public class AchievementDto
{
    [JsonPropertyName("ID")]
    public int Id {get; set;}

    [JsonPropertyName("BadgeName")]
    public string BadgeName {get; set;} = "";
    
    [JsonPropertyName("Title")]
    public string Name {get; set;} = "";

    [JsonPropertyName("Description")]
    public string Description {get; set;} = "";

    [JsonPropertyName("DateEarned")]
    public string DateEarned {get; set;} = "";

    [JsonPropertyName("DateEarnedHardcore")]
    public string DateEarnedHardcore {get; set;} = "";
}