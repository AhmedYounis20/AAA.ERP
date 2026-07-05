using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.IntegrationTests.Inventory;

public class InventoryTransferIntegrationTests : IAsyncLifetime
{
    private IntegrationTestWebApplicationFactory _factory = null!;
    private AsyncServiceScope _scope;

    public async Task InitializeAsync()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _scope = _factory.Services.CreateAsyncScope();
        await InventoryTestDataSeeder.SeedAsync(_scope.ServiceProvider, initialStockBalance: 100);
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Direct_transfer_moves_stock_between_branches_and_posts_gl()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IInventoryTransferService>();
        const decimal transferQuantity = 30;

        var result = await service.Create(new InventoryTransferCreateCommand
        {
            SourceBranchId = InventoryTestIds.Branch1Id,
            DestinationBranchId = InventoryTestIds.Branch2Id,
            TransferType = InventoryTransferType.Direct,
            Items =
            [
                new InventoryTransferItemCreateCommand
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = transferQuantity
                }
            ]
        });

        result.IsSuccess.Should().BeTrue(result.Errors?.FirstOrDefault()?.MessageKey);
        result.Result!.Status.Should().Be(InventoryTransferStatus.Approved);

        var sourceStock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        var destStock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch2Id);

        sourceStock.Should().Be(70);
        destStock.Should().Be(30);

        var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entryCount.Should().Be(1);
    }

    [Fact]
    public async Task Staged_transfer_moves_stock_on_create_and_complete_on_approve()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IInventoryTransferService>();
        const decimal transferQuantity = 25;

        var createResult = await service.Create(new InventoryTransferCreateCommand
        {
            SourceBranchId = InventoryTestIds.Branch1Id,
            DestinationBranchId = InventoryTestIds.Branch2Id,
            TransferType = InventoryTransferType.Conditional,
            Items =
            [
                new InventoryTransferItemCreateCommand
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = transferQuantity
                }
            ]
        });

        createResult.IsSuccess.Should().BeTrue(createResult.Errors?.FirstOrDefault()?.MessageKey);
        createResult.Result!.Status.Should().Be(InventoryTransferStatus.Pending);

        var sourceAfterCreate = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        var stagingAfterCreate = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.StockUnderTransferBranchId);
        var destAfterCreate = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch2Id);

        sourceAfterCreate.Should().Be(75);
        stagingAfterCreate.Should().Be(25);
        destAfterCreate.Should().Be(0);

        var entriesAfterCreate = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entriesAfterCreate.Should().Be(1);

        var approveResult = await service.ApproveTransfer(createResult.Result.Id);
        approveResult.IsSuccess.Should().BeTrue(approveResult.Errors?.FirstOrDefault()?.MessageKey);
        approveResult.Result!.Status.Should().Be(InventoryTransferStatus.Approved);

        var stagingAfterApprove = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.StockUnderTransferBranchId);
        var destAfterApprove = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch2Id);

        stagingAfterApprove.Should().Be(0);
        destAfterApprove.Should().Be(25);

        var entriesAfterApprove = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entriesAfterApprove.Should().Be(2);
    }

    [Fact]
    public async Task Staged_transfer_via_MediatR_create_and_approve_posts_two_gl_entries()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        const decimal transferQuantity = 20;

        var createResult = await sender.Send(new InventoryTransferCreateCommand
        {
            SourceBranchId = InventoryTestIds.Branch1Id,
            DestinationBranchId = InventoryTestIds.Branch2Id,
            TransferType = InventoryTransferType.Conditional,
            Items =
            [
                new InventoryTransferItemCreateCommand
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = transferQuantity
                }
            ]
        });

        createResult.IsSuccess.Should().BeTrue(createResult.Errors?.FirstOrDefault()?.MessageKey);
        createResult.Result!.Status.Should().Be(InventoryTransferStatus.Pending);

        var approveResult = await sender.Send(new ApproveInventoryTransferCommand(createResult.Result.Id));
        approveResult.IsSuccess.Should().BeTrue(approveResult.Errors?.FirstOrDefault()?.MessageKey);
        approveResult.Result!.Status.Should().Be(InventoryTransferStatus.Approved);

        var entriesAfterApprove = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entriesAfterApprove.Should().Be(2);
    }
}
