namespace Shared.Application.Seeding;

public interface IModuleSeeder
{
    string Name { get; }
    Task SeedAsync(CancellationToken cancellationToken = default);
}


