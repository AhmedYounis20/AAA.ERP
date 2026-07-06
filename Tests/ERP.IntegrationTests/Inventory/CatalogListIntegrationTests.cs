using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Inventory.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Application.DTOs;

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
        var queries = _scope.ServiceProvider.GetRequiredService<IProductQueries>();

        var result = await queries.GetAll();

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.Should().Contain(p => p.Id == InventoryTestIds.ProductId);
    }

    [Fact]
    public async Task Variant_full_list_and_paginated_list_return_seeded_variant()
    {
        var queries = _scope.ServiceProvider.GetRequiredService<IVariantQueries>();

        var fullList = await queries.GetAll();
        fullList.IsSuccess.Should().BeTrue();
        fullList.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);

        var paginated = await queries.GetPaginated(
            new SettingFilterDto { PageNumber = 1, PageSize = 10 });
        paginated.IsSuccess.Should().BeTrue();
        paginated.Result!.Items.Should().Contain(v => v.Id == InventoryTestIds.VariantId);
    }

    [Fact]
    public async Task Variant_by_product_and_search_return_seeded_variant()
    {
        var queries = _scope.ServiceProvider.GetRequiredService<IVariantQueries>();

        var byProduct = await queries.GetVariantsByProductId(InventoryTestIds.ProductId);
        byProduct.IsSuccess.Should().BeTrue();
        byProduct.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);

        var search = await queries.SearchVariants("Test Product");
        search.IsSuccess.Should().BeTrue();
        search.Result.Should().Contain(v => v.Id == InventoryTestIds.VariantId);
    }
}
