using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RetroAchievCollection.Models;

public class GameModel
{
    [JsonPropertyName("id")]
    public int Id {get; set;}

    [JsonPropertyName("consoleId")]
    public int ConsoleId {get; set;}

    [JsonPropertyName("name")]
    public string Name {get; set;} = "";

    [JsonPropertyName("publisher")]
    public string Publisher {get; set;} = "";

    [JsonPropertyName("developer")]
    public string Developer {get; set;} = "";

    [JsonPropertyName("genre")]
    public string Genre {get; set;} = "";

    [JsonPropertyName("released")]
    public string Released {get; set;} = "";

    [JsonPropertyName("totalAchievements")]
    public int TotalAchievements {get; set;}

    [JsonPropertyName("totalAchievementsCompleted")]
    public int TotalAchievementsCompleted {get; set;}

    [JsonPropertyName("totalAchievementsCompletedHardcore")]
    public int totalAchievementsCompletedHardcore {get; set;}

    [JsonPropertyName("isFavorite")]
    public bool IsFavorite {get; set;}

    [JsonPropertyName("playCommand")]
    public string PlayCommand {get; set;} = "";

    [JsonPropertyName("imagePath")]
    public string ImagePath {get; set;} = "";

    [JsonPropertyName("achievements")]
    public List<AchievementModel>? Achievements {get; set;}
}