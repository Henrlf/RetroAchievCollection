using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.Commands.Game;

public class SynchronizeConsoleGamesCommand
{
    private readonly RetroAchievementsService RetroAchievementsService;
    private readonly GameService GameService = new();

    public int ConsoleCodeIntegration {get; set;}

    public SynchronizeConsoleGamesCommand(ConfigurationService configurationService)
    {
        RetroAchievementsService = new RetroAchievementsService(configurationService.getConfigurationModel());
    }

    public async Task Execute()
    {
        if (ConsoleCodeIntegration == 0)
        {
            throw new ArgumentException("Console Id must be specified!");
        }

        var gamesDto = await RetroAchievementsService.getConsoleGamesAsync(ConsoleCodeIntegration);
        var semaphore = new SemaphoreSlim(2);

        await Task.WhenAll(gamesDto.Select(async simpleGameDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                var infoGameDto = await RetroAchievementsService.getGameAndAchievementsAsync(simpleGameDto.CodeIntegration);
                GameModel gameModel = await GameService.SaveGameDto(infoGameDto, ConsoleCodeIntegration);

                await SaveAchievements(infoGameDto.Achievements.Values.ToList(), gameModel);
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }

    protected async Task SaveAchievements(List<AchievementDto> achievementsDto, GameModel gameModel)
    {
        var semaphore = new SemaphoreSlim(25);

        await Task.WhenAll(achievementsDto.Select(async achievementDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                await GameService.SaveAchievementDto(achievementDto, gameModel);
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }
}