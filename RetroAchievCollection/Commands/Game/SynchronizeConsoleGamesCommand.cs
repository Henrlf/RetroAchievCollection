using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.Commands.Game;

public class SynchronizeConsoleGamesCommand
{
    private readonly RetroAchievementsService RetroAchievementsService;
    private readonly GameService GameService = new();

    public int ConsoleId {get; set;}

    public SynchronizeConsoleGamesCommand(ConfigurationService configurationService)
    {
        RetroAchievementsService = new RetroAchievementsService(configurationService.getConfigurationModel());
    }

    public async Task execute()
    {
        if (ConsoleId == 0)
        {
            throw new ArgumentException("Console Id must be specified!");
        }

        var gamesDto = await RetroAchievementsService.getConsoleGamesAsync(ConsoleId);
        var semaphore = new SemaphoreSlim(2);

        await Task.WhenAll(gamesDto.Select(async simpleGameDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                var infoGameDto = await RetroAchievementsService.getGameAndAchievementsAsync(simpleGameDto.Id);
                await GameService.SaveGameAndAchievements(infoGameDto);
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }
}