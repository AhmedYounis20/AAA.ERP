using System.Net;
using ERP.BuildingBlocks.Persistence.Repositories;
using ERP.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.UnitTests.Persistence;

public class TransactionCoordinatorTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    [Fact]
    public async Task ExecuteAsync_commits_when_action_succeeds()
    {
        var coordinator = new TransactionCoordinator(_unitOfWork.Object);

        var result = await coordinator.ExecuteAsync(_ => Task.FromResult(new ApiResponse<int>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = 42
        }));

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().Be(42);
        _unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_rolls_back_when_action_fails()
    {
        var coordinator = new TransactionCoordinator(_unitOfWork.Object);

        var result = await coordinator.ExecuteAsync(_ => Task.FromResult(new ApiResponse<int>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest
        }));

        result.IsSuccess.Should().BeFalse();
        _unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}
