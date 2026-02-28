using System.Collections.ObjectModel;
using RetroAchievCollection.RetroAchievements.Dtos;
using RetroAchievCollection.Services.User;

namespace RetroAchievCollection.RetroAchievements.Services;

public class RetroAchievementsService : BaseService
{
    public ConfigurationService ConfigurationService {get;} = new();

    public Collection<ConsoleDto> getConsoles()
    {
        Collection<ConsoleDto> consoles = new();

        return consoles;
    }
}