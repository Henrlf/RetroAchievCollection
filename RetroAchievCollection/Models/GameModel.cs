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
    
    [JsonPropertyName("isFavorite")]
    public bool IsFavorite {get; set;}
    
    [JsonPropertyName("playCommand")]
    public bool PlayCommand {get; set;}
    
    [JsonPropertyName("imagePath")]
    public string ImagePath {get; set;} = ""; // 
    
    // [JsonPropertyName("achievements")]
    // public Collection<AchievementModel> Achievements {get; set;}
}