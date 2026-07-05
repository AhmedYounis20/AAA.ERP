using FluentAssertions;
using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;
using Inventory.Infrastructure.Handlers.InventoryTransfers;
using ERP.BuildingBlocks.Application.Responses;
using Moq;

namespace Inventory.UnitTests.Handlers;

public class ApproveInventoryTransferCommandHandlerTests
{
    [Fact]
    public async Task Handle_DelegatesToTransferServiceApprove()
    {
        var transferId = Guid.NewGuid();
        var transferService = new Mock<IInventoryTransferService>();
        transferService
            .Setup(s => s.ApproveTransfer(transferId))
            .ReturnsAsync(new ApiResponse<InventoryTransfer>
            {
                IsSuccess = true,
                Result = new InventoryTransfer { Id = transferId, Status = InventoryTransferStatus.Approved }
            });

        var handler = new ApproveInventoryTransferCommandHandler(transferService.Object);
        var result = await handler.Handle(new ApproveInventoryTransferCommand(transferId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.Status.Should().Be(InventoryTransferStatus.Approved);
        transferService.Verify(s => s.ApproveTransfer(transferId), Times.Once);
    }
}
