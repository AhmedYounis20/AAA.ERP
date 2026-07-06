using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.IntegrationTests.Inventory;

public class InventoryCatalogSettingsIntegrationTests : IAsyncLifetime
{
  private IntegrationTestWebApplicationFactory _factory = null!;
  private AsyncServiceScope _scope;

  public async Task InitializeAsync()
  {
    _factory = new IntegrationTestWebApplicationFactory();
    _scope = _factory.Services.CreateAsyncScope();
    await InventoryTestDataSeeder.SeedAsync(_scope.ServiceProvider);
  }

  public async Task DisposeAsync()
  {
    await _scope.DisposeAsync();
    await _factory.DisposeAsync();
  }

  [Fact]
  public async Task Packing_unit_list_returns_seeded_unit()
  {
    var queries = _scope.ServiceProvider.GetRequiredService<IPackingUnitQueries>();

    var result = await queries.GetAll();

    result.IsSuccess.Should().BeTrue();
    result.Result.Should().Contain(p => p.Id == InventoryTestIds.PackingUnitId && p.Name == "Piece");
  }

  [Fact]
  public async Task Create_color_via_MediatR_persists_and_appears_in_list()
  {
    var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    var service = _scope.ServiceProvider.GetRequiredService<IColorService>();

    var response = await sender.Send(new ColorCreateCommand
    {
      Name = "Crimson",
      NameSecondLanguage = "Crimson",
      ColorValue = "#DC143C"
    });

    response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);

    var list = await service.ReadAll();
    list.IsSuccess.Should().BeTrue();
    list.Result.Should().Contain(c => c.Name == "Crimson");
  }
}
