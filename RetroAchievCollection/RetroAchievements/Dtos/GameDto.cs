using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RetroAchievCollection.RetroAchievements.Dtos;

public class GameDto
{
    [JsonPropertyName("ID")]
    public int Id {get; set;}

    [JsonPropertyName("ConsoleID")]
    public int ConsoleId {get; set;}

    [JsonPropertyName("Title")]
    public string Name {get; set;} = "";

    [JsonPropertyName("Publisher")]
    public string Publisher {get; set;} = "";

    [JsonPropertyName("Developer")]
    public string Developer {get; set;} = "";

    [JsonPropertyName("Genre")]
    public string Genre {get; set;} = "";

    [JsonPropertyName("Released")]
    public string Released {get; set;} = "";

    [JsonPropertyName("NumAchievements")]
    public int NumAchievements {get; set;} = 0;

    [JsonPropertyName("NumAwardedToUser")]
    public int NumAchievementsCompleted {get; set;} = 0;

    [JsonPropertyName("NumAwardedToUserHardcore")]
    public int NumAchievementsCompletedHardcore {get; set;} = 0;

    [JsonPropertyName("ImageIcon")]
    public string ImageUrl {get; set;} = "";
    
    [JsonPropertyName("Achievements")]
    public List<AchievementDto>? Achievements {get; set;}
}