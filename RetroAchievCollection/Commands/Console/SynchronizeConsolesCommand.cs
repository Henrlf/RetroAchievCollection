using RetroAchievCollection.RetroAchievements.Services;

namespace RetroAchievCollection.Commands.Console;

public class SynchronizeConsolesCommand
{
    public readonly RetroAchievementsService retroAchievementsService = new();

    public bool execute()
    {
        var consoles = retroAchievementsService.getConsoles();
        
        return true;
    }
}