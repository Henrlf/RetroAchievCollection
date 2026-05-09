using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Game;

namespace RetroAchievCollection.Commands.Game;

public class SynchronizeConsoleGamesCommand
{
    private readonly GameService GameService = new();

    public int ConsoleCodeIntegration {get; set;}

    public async Task Execute()
    {
        if (ConsoleCodeIntegration == 0)
        {
            throw new ArgumentException("Console Id must be specified!");
        }

        ConfigurationRepository configurationRepository = new();
        RetroAchievementsService retroAchievService = new RetroAchievementsService(await configurationRepository.GetConfiguration());

        var gamesDto = await retroAchievService.getConsoleGamesAsync(ConsoleCodeIntegration);
        var semaphore = new SemaphoreSlim(2);

        await Task.WhenAll(gamesDto.Select(async simpleGameDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                var infoGameDto = await retroAchievService.getGameAndAchievementsAsync(simpleGameDto.CodeIntegration);
                GameModel gameModel = await GameService.SaveGameDto(infoGameDto, ConsoleCodeIntegration);

                await GameService.SaveAchievementsDto(infoGameDto.Achievements.Values.ToList(), gameModel);
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }
}