using Identity.Domain.Models;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Application.Seeding;
using Shared.Domain.BaseEntities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Identity.Infrastructure.Seeding;

public class IdentityModuleSeeder : IModuleSeeder
{
    private readonly IdentityDbContext _context;
    private readonly ILogger<IdentityModuleSeeder> _logger;
    public string Name => "Identity";

    public IdentityModuleSeeder(IdentityDbContext db, ILogger<IdentityModuleSeeder> logger)
    {
        _context = db;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

            var basePath = Path.Combine(AppContext.BaseDirectory, "Seeding", "Data");
            await ImportIdentityData(basePath);
            await transaction.CommitAsync();

        }
        catch (Exception ex) {

            await transaction.RollbackAsync();
            _logger.LogError(ex.ToString());
        }
    }
    private async Task ImportIdentityData(string folder)
    {
        await ImportUsers(folder);
        await ImportRoles(folder);
        await ImportUserRoles(folder);
    }

    private async Task ImportUserRoles(string basePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine(basePath, "AspNetUserRoles.json"));
            List<IdentityUserRole<string>> entities =
                JsonConvert.DeserializeObject<List<IdentityUserRole<string>>>(json) ?? new();
            _context.Set<IdentityUserRole<string>>()
                .RemoveRange(_context.Set<IdentityUserRole<string>>().AsNoTracking().ToList());
            await _context.Set<IdentityUserRole<string>>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportRoles(string basePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine(basePath, "AspNetRoles.json"));
            List<IdentityRole> entities = JsonConvert.DeserializeObject<List<IdentityRole>>(json) ?? new();
            _context.Set<IdentityRole>().RemoveRange(_context.Set<IdentityRole>().AsNoTracking().ToList());
            await _context.Set<IdentityRole>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportUsers(string basePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine(basePath, "AspNetUsers.json"));
            List<ApplicationUser> entities = JsonConvert.DeserializeObject<List<ApplicationUser>>(json) ?? new();
            _context.Set<ApplicationUser>().RemoveRange(_context.Set<ApplicationUser>().AsNoTracking().ToList());
            await _context.Set<ApplicationUser>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}


