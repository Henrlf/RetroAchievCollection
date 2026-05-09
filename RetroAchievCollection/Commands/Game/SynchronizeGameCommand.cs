using System;
using System.Linq;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Game;

namespace RetroAchievCollection.Commands.Game;

public class SynchronizeGameCommand
{
    public readonly GameService GameService = new();

    public int GameCodeIntegration {get; set;}
    public GameModel? GameModel {get; protected set;}

    public async Task execute()
    {
        if (GameCodeIntegration == 0)
        {
            throw new ArgumentException("Game Id must be specified!");
        }

        ConfigurationRepository configurationRepository = new();
        RetroAchievementsService retroAchievService = new RetroAchievementsService(await configurationRepository.GetConfiguration());
        
        var gameDto = await retroAchievService.getGameAndAchievementsAsync(GameCodeIntegration);

        GameModel = await GameService.SaveGameDto(gameDto, gameDto.ConsoleId);

        await GameService.SaveAchievementsDto(gameDto.Achievements.Values.ToList(), GameModel);
    }
}