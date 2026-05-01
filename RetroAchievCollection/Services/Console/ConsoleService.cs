using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Console;

public class ConsoleService : BaseService
{
    public async Task<ConsoleModel?> GetConsole(Guid consoleId)
    {
        using var db = new AppDbContext();
        return await db.Consoles.FindAsync(consoleId);
    }

    public async Task<List<ConsoleModel>> GetConsoles()
    {
        using var db = new AppDbContext();

        return await db.Consoles
            .ToListAsync();
    }

    public async Task SaveConsoleDto(ConsoleDto consoleDto)
    {
        using var db = new AppDbContext();

        var consoleModel = db.Consoles.FirstOrDefault(c => c.CodeIntegration == consoleDto.CodeIntegration);

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
            db.Consoles.Add(consoleModel);
        }
        else
        {
            db.Consoles.Update(consoleModel);
        }

        await db.SaveChangesAsync();
    }
}