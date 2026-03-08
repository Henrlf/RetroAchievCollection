using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.Commands.Console;

public class SynchronizeConsolesCommand
{
    public readonly RetroAchievementsService RetroAchievementsService;
    private readonly ConsoleService ConsoleService = new();
    public readonly GameService GameService = new();

    public SynchronizeConsolesCommand(ConfigurationService configurationService)
    {
        RetroAchievementsService = new RetroAchievementsService(configurationService.getConfigurationModel());
    }

    public async Task execute()
    {
        var consolesDto = await RetroAchievementsService.getConsolesAsync(1);
        await ConsoleService.SaveConsoles(consolesDto);

        foreach (var consoleDto in consolesDto)
        {
            var gamesDto = await RetroAchievementsService.getConsoleGamesAsync(consoleDto.ConsoleId);
            var semaphore = new SemaphoreSlim(10);

            await Task.WhenAll(gamesDto.Select(async gameDto =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await GameService.SaveGame(gameDto);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }
}