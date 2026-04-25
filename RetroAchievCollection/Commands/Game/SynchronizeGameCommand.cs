using System;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.Commands.Game;

public class SynchronizeGameCommand
{
    public readonly RetroAchievementsService RetroAchievementsService;
    public readonly GameService GameService = new();

    public int GameCodeIntegration {get; set;}
    public GameModel? GameModel {get; protected set;}

    public SynchronizeGameCommand(ConfigurationService configurationService)
    {
        RetroAchievementsService = new RetroAchievementsService(configurationService.getConfigurationModel());
    }

    public async Task execute()
    {
        if (GameCodeIntegration == 0)
        {
            throw new ArgumentException("Game Id must be specified!");
        }

        var gameDto = await RetroAchievementsService.getGameAndAchievementsAsync(GameCodeIntegration);

        GameModel = await GameService.SaveGameDto(gameDto, gameDto.ConsoleId);
    }
}