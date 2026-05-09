using System;
using System.IO;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Console;

public class ConsoleService : BaseService
{
    public async Task SaveConsoleDto(ConsoleDto consoleDto)
    {
        ConsoleRepository consoleRepository = new();
        ConsoleModel? consoleModel = await consoleRepository.GetConsoleByCodeIntegration(consoleDto.CodeIntegration);

        if (consoleModel == null)
        {
            consoleModel = new ConsoleModel();
        }

        consoleModel.CodeIntegration = consoleDto.CodeIntegration;
        consoleModel.Name = consoleDto.Name;

        if ((string.IsNullOrWhiteSpace(consoleModel.ImagePath) || !File.Exists(consoleModel.ImagePath))
            && !string.IsNullOrWhiteSpace(consoleDto.ImageUrl))
        {
            try
            {
                var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
                var imagePath = Path.Combine("images", "console", consoleModel.CodeIntegration + extension);

                await SaveImageAsync(consoleDto.ImageUrl, Path.Combine(MainDirectory, imagePath));
                consoleModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        if (consoleModel.Id == Guid.Empty)
        {
            await consoleRepository.InsertConsole(consoleModel);
        }
        else
        {
            await consoleRepository.UpdateConsole(consoleModel);
        }
    }
}