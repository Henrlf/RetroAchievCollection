using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Repositories;

public class ConsoleRepository
{
    public async Task<ConsoleModel?> GetConsole(Guid consoleId)
    {
        using var db = new AppDbContext();
        return await db.Consoles.FindAsync(consoleId);
    }

    public async Task<ConsoleModel?> GetConsoleByCodeIntegration(int codeIntegration)
    {
        using var db = new AppDbContext();
        return await db.Consoles.SingleOrDefaultAsync(c => c.CodeIntegration == codeIntegration);
    }

    public async Task<List<ConsoleModel>> GetConsoles()
    {
        using var db = new AppDbContext();
        return await db.Consoles.ToListAsync();
    }

    public async Task InsertConsole(ConsoleModel console)
    {
        using var db = new AppDbContext();
        db.Consoles.Add(console);
        await db.SaveChangesAsync();
    }

    public async Task UpdateConsole(ConsoleModel console)
    {
        using var db = new AppDbContext();
        db.Consoles.Update(console);
        await db.SaveChangesAsync();
    }
}