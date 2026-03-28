using System.Threading.Tasks;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.Commands.Console;

public class SynchronizeConsolesCommand
{
    public readonly RetroAchievementsService RetroAchievementsService;
    private readonly ConsoleService ConsoleService = new();

    public SynchronizeConsolesCommand(ConfigurationService configurationService)
    {
        RetroAchievementsService = new RetroAchievementsService(configurationService.getConfigurationModel());
    }

    public async Task execute()
    {
        var consolesDto = await RetroAchievementsService.getConsolesAsync(1);
        await ConsoleService.SaveConsoles(consolesDto);
    }
}