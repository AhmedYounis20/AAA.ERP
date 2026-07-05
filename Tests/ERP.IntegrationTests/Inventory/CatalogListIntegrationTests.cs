using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Inventory.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Shared.DTOs;

namespace ERP.IntegrationTests.Inventory;

/// <summary>
/// Regression coverage for catalog list queries (Phase 4.2.5 — N+1 fixes in Product/Variant repositories).
/// </summary>
public class CatalogListIntegrationTests : IAsyncLifetime
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
    public async Task Product_list_returns_seeded_product_without_error()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IProductService>();

        var result = await service.GetProductDtos();

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.Should().Contain(p => p.Id == InventoryTestIds.ProductId);
    }

    [Fact]
    public async Task Variant_full_list_and_paginated_list_return_seeded_variant()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IVariantService>();

        var fullList = await service.GetVariantDtos();
        fullList.IsSuccess.Should().BeTrue();
        fullList.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);

        var paginated = await service.ReadAllDtosPaginated(
            new SettingFilterDto { PageNumber = 1, PageSize = 10 });
        paginated.IsSuccess.Should().BeTrue();
        paginated.Result!.Items.Should().Contain(v => v.Id == InventoryTestIds.VariantId);
    }

    [Fact]
    public async Task Variant_by_product_and_search_return_seeded_variant()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IVariantService>();

        var byProduct = await service.GetVariantsByProductId(InventoryTestIds.ProductId);
        byProduct.IsSuccess.Should().BeTrue();
        byProduct.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);

        var search = await service.SearchVariants("Test Product");
        search.IsSuccess.Should().BeTrue();
        search.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);
    }
}
