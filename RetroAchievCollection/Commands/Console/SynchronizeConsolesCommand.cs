using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.RetroAchievements.Services;
using RetroAchievCollection.Services.Console;

namespace RetroAchievCollection.Commands.Console;

public class SynchronizeConsolesCommand
{
    private readonly ConsoleService ConsoleService = new();

    public async Task execute()
    {
        ConfigurationRepository configurationRepository = new();
        RetroAchievementsService retroAchievService = new RetroAchievementsService(await configurationRepository.GetConfiguration());

        var consolesDto = await retroAchievService.getConsolesAsync(1);
        var semaphore = new SemaphoreSlim(5);

        await Task.WhenAll(consolesDto.Select(async consoleDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                await ConsoleService.SaveConsoleDto(consoleDto);
            }
            finally
            {
                semaphore.Release();
            }
        }));

    }
}