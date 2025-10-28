using Microsoft.AspNetCore.Mvc;
using Shared.Application.Seeding;

namespace ERP.MainAPI.Controllers;

[ApiController]
[Route("api/dbwork")] 
public class DbWorkController : ControllerBase
{
    private readonly IEnumerable<IModuleSeeder> _seeders;

    public DbWorkController(IEnumerable<IModuleSeeder> seeders)
    {
        _seeders = seeders;
    }

    [HttpPost("seed-all")] 
    public async Task<IActionResult> SeedAll()
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync();
        }
        return Ok("Seeding completed for all modules");
    }
}


