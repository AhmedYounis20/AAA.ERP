using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.IntegrationTests.Accounting;

public class CurrencyIntegrationTests : IAsyncLifetime
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
  public async Task Create_currency_via_MediatR_persists_and_can_be_read_back()
  {
    var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    var repository = _scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();

    var command = new CurrencyCreateCommand
    {
      Name = "USD",
      NameSecondLanguage = "USD",
      Symbol = "USD",
      ExchangeRate = 48.5m,
      IsDefault = false,
      IsActive = true
    };

    var response = await sender.Send(command);

    response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
    response.Result.Should().NotBeNull();
    response.Result!.Symbol.Should().Be("USD");

    var persisted = await repository.Get(response.Result.Id);
    persisted.Should().NotBeNull();
    persisted!.ExchangeRate.Should().Be(48.5m);
  }

  [Fact]
  public async Task Default_currency_from_seed_is_readable()
  {
    var repository = _scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();

    var currency = await repository.Get(InventoryTestIds.CurrencyId);

    currency.Should().NotBeNull();
    currency!.IsDefault.Should().BeTrue();
    currency.Symbol.Should().Be("EGP");
  }
}
