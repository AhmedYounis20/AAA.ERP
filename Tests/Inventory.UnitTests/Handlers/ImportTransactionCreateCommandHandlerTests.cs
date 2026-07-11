using FluentAssertions;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Infrastructure.Handlers.InventoryTransactions;
using ERP.BuildingBlocks.Application.Responses;
using Moq;

namespace Inventory.UnitTests.Handlers;

public class ImportTransactionCreateCommandHandlerTests
{
    [Fact]
    public async Task Handle_MapsToReceiptAndCallsInventoryTransactionService()
    {
        var workflow = new Mock<IInventoryTransactionWorkflowService>();
        workflow
            .Setup(s => s.Create(It.IsAny<InventoryTransactionCreateCommand>(), true))
            .ReturnsAsync(new ApiResponse<InventoryTransaction>
            {
                IsSuccess = true,
                Result = new InventoryTransaction { Id = Guid.NewGuid() }
            });

        var handler = new ImportTransactionCreateCommandHandler(workflow.Object);
        var command = new ImportTransactionCreateCommand
        {
            TransactionDate = DateTime.UtcNow,
            BranchId = Guid.NewGuid(),
            TransactionPartyId = Guid.NewGuid(),
            Items = []
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        workflow.Verify(s => s.Create(
            It.Is<InventoryTransactionCreateCommand>(c =>
                c.TransactionType == InventoryTransactionType.Receipt),
            true), Times.Once);
    }
}
