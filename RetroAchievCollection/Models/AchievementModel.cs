namespace RetroAchievCollection.Models;

public class AchievementModel
{
    public int Id {get; set;} = 0;
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public string ImagePath {get; set;} = "";
    public bool IsCompleted {get; set;} = false;
}