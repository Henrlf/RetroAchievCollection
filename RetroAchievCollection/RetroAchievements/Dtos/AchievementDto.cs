using System;
using System.Text.Json.Serialization;

namespace RetroAchievCollection.RetroAchievements.Dtos;

public class AchievementDto
{
    [JsonPropertyName("ID")]
    public int Id {get; set;}

    [JsonPropertyName("Title")]
    public string Name {get; set;} = "";

    [JsonPropertyName("Description")]
    public string Description {get; set;} = "";

    [JsonPropertyName("DateEarned")]
    public DateTime? DateEarned {get; set;}

    [JsonPropertyName("DateEarnedHardcore")]
    public DateTime? DateEarnedHardcore {get; set;}
}