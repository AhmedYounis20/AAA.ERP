using System.Linq.Expressions;
using System.Net;
using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Persistence.Repositories;
using FluentAssertions;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Infrastructure.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.UnitTests.Handlers;

/// <summary>
/// Unit tests for receipt/issue GL posting via <see cref="InventoryTransactionWorkflowService"/>
/// with mocked accounting boundary and persistence.
/// </summary>
public class InventoryGlPostingHandlerTests
{
  private readonly Mock<IInventoryTransactionRepository> _repository = new();
  private readonly Mock<IStockBalanceService> _stockBalance = new();
  private readonly Mock<ITransactionCoordinator> _coordinator = new();
  private readonly Mock<IAccountingIntegrationService> _accounting = new();

  private readonly Guid _branchId = Guid.Parse("10000000-0000-0000-0000-000000000001");
  private readonly Guid _partyId = Guid.Parse("10000000-0000-0000-0000-000000000002");
  private readonly Guid _periodId = Guid.Parse("10000000-0000-0000-0000-000000000003");
  private readonly Guid _variantId = Guid.Parse("10000000-0000-0000-0000-000000000004");
  private readonly Guid _packingUnitId = Guid.Parse("10000000-0000-0000-0000-000000000005");

  public InventoryGlPostingHandlerTests()
  {
    _coordinator
      .Setup(c => c.ExecuteAsync(
        It.IsAny<Func<CancellationToken, Task<ApiResponse<InventoryTransaction>>>>(),
        It.IsAny<CancellationToken>()))
      .Returns<Func<CancellationToken, Task<ApiResponse<InventoryTransaction>>>, CancellationToken>(
        (action, ct) => action(ct));

    _repository.Setup(r => r.GetQuery()).Returns(CreateMockDbSet(new List<InventoryTransaction>()));

    _accounting
      .Setup(a => a.GetOpenPeriodForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(new ApiResponse<FinancialPeriodInfo>
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK,
        Result = new FinancialPeriodInfo(_periodId, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1), "2026")
      });

    _stockBalance
      .Setup(s => s.UpdateStockBalance(
        It.IsAny<Guid>(),
        It.IsAny<Guid>(),
        It.IsAny<Guid>(),
        It.IsAny<decimal>(),
        It.IsAny<decimal>(),
        It.IsAny<bool>()))
      .ReturnsAsync(new ApiResponse<bool> { IsSuccess = true, Result = true });

    _accounting
      .Setup(a => a.PostInventoryTransactionEntryAsync(
        It.IsAny<InventoryTransactionPostingRequest>(),
        It.IsAny<CancellationToken>()))
      .ReturnsAsync(new ApiResponse<Guid>
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.Created,
        Result = Guid.NewGuid()
      });
  }

  [Fact]
  public async Task Receipt_posts_gl_with_receipt_direction_and_transaction_id_as_source_document()
  {
    var transactionId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    InventoryTransactionPostingRequest? captured = null;

    _repository
      .Setup(r => r.Add(It.IsAny<InventoryTransaction>()))
      .ReturnsAsync((InventoryTransaction t) =>
      {
        t.Id = transactionId;
        return t;
      });

    _accounting
      .Setup(a => a.PostInventoryTransactionEntryAsync(
        It.IsAny<InventoryTransactionPostingRequest>(),
        It.IsAny<CancellationToken>()))
      .Callback<InventoryTransactionPostingRequest, CancellationToken>((req, _) => captured = req)
      .ReturnsAsync(new ApiResponse<Guid>
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.Created,
        Result = Guid.NewGuid()
      });

    var service = CreateService();
    var result = await service.Create(CreateCommand(InventoryTransactionType.Receipt, quantity: 5, totalCost: 50), isValidate: false);

    result.IsSuccess.Should().BeTrue();
    captured.Should().NotBeNull();
    captured!.SourceDocumentId.Should().Be(transactionId);
    captured.Direction.Should().Be(InventoryGlDirection.Receipt);
    captured.TotalAmount.Should().Be(50m);
    captured.BranchId.Should().Be(_branchId);
    captured.TransactionPartyId.Should().Be(_partyId);
  }

  [Fact]
  public async Task Issue_posts_gl_with_issue_direction_when_stock_is_sufficient()
  {
    var transactionId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    InventoryTransactionPostingRequest? captured = null;

    _repository
      .Setup(r => r.Add(It.IsAny<InventoryTransaction>()))
      .ReturnsAsync((InventoryTransaction t) =>
      {
        t.Id = transactionId;
        return t;
      });

    _stockBalance
      .Setup(s => s.GetCurrentBalance(_variantId, _packingUnitId, _branchId))
      .ReturnsAsync(new ApiResponse<decimal> { IsSuccess = true, Result = 100 });

    _accounting
      .Setup(a => a.PostInventoryTransactionEntryAsync(
        It.IsAny<InventoryTransactionPostingRequest>(),
        It.IsAny<CancellationToken>()))
      .Callback<InventoryTransactionPostingRequest, CancellationToken>((req, _) => captured = req)
      .ReturnsAsync(new ApiResponse<Guid>
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.Created,
        Result = Guid.NewGuid()
      });

    var service = CreateService();
    var result = await service.Create(CreateCommand(InventoryTransactionType.Issue, quantity: 4, totalCost: 40), isValidate: false);

    result.IsSuccess.Should().BeTrue();
    captured!.Direction.Should().Be(InventoryGlDirection.Issue);
    captured.SourceDocumentId.Should().Be(transactionId);
    captured.TotalAmount.Should().Be(40m);
  }

  [Fact]
  public async Task Issue_with_insufficient_stock_does_not_post_gl()
  {
    _stockBalance
      .Setup(s => s.GetCurrentBalance(_variantId, _packingUnitId, _branchId))
      .ReturnsAsync(new ApiResponse<decimal> { IsSuccess = true, Result = 1 });

    var service = CreateService();
    var result = await service.Create(CreateCommand(InventoryTransactionType.Issue, quantity: 5, totalCost: 50), isValidate: false);

    result.IsSuccess.Should().BeFalse();
    result.Errors.Should().Contain(e => e.MessageKey == ErrorMessages.InsufficientStockBalance);
    _accounting.Verify(
      a => a.PostInventoryTransactionEntryAsync(It.IsAny<InventoryTransactionPostingRequest>(), It.IsAny<CancellationToken>()),
      Times.Never);
    _repository.Verify(r => r.Add(It.IsAny<InventoryTransaction>()), Times.Never);
  }

  [Fact]
  public async Task Gl_posting_failure_returns_failure_response()
  {
    _repository
      .Setup(r => r.Add(It.IsAny<InventoryTransaction>()))
      .ReturnsAsync((InventoryTransaction t) =>
      {
        t.Id = Guid.NewGuid();
        return t;
      });

    _accounting
      .Setup(a => a.PostInventoryTransactionEntryAsync(
        It.IsAny<InventoryTransactionPostingRequest>(),
        It.IsAny<CancellationToken>()))
      .ReturnsAsync(new ApiResponse<Guid>
      {
        IsSuccess = false,
        StatusCode = HttpStatusCode.BadRequest,
        Errors = [new MessageTemplate { MessageKey = ErrorMessages.PartyAccountNotFound }]
      });

    var service = CreateService();
    var result = await service.Create(CreateCommand(InventoryTransactionType.Receipt, quantity: 2, totalCost: 20), isValidate: false);

    result.IsSuccess.Should().BeFalse();
    result.Errors.Should().Contain(e => e.MessageKey == ErrorMessages.PartyAccountNotFound);
  }

  private InventoryTransactionWorkflowService CreateService()
    => new(_repository.Object, _stockBalance.Object, _coordinator.Object, _accounting.Object);

  private InventoryTransactionCreateCommand CreateCommand(
    InventoryTransactionType type,
    decimal quantity,
    decimal totalCost)
    => new()
    {
      TransactionType = type,
      TransactionDate = new DateTime(2026, 6, 1),
      BranchId = _branchId,
      TransactionPartyId = _partyId,
      Items =
      [
        new InventoryTransactionItemCreateDto
        {
          VariantId = _variantId,
          PackingUnitId = _packingUnitId,
          Quantity = quantity,
          TotalCost = totalCost
        }
      ]
    };

  private static DbSet<T> CreateMockDbSet<T>(List<T> data) where T : class
  {
    var queryable = data.AsQueryable();
    var mock = new Mock<DbSet<T>>();
    mock.As<IAsyncEnumerable<T>>()
      .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
      .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));
    mock.As<IQueryable<T>>()
      .Setup(m => m.Provider)
      .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
    mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
    mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
    mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
    return mock.Object;
  }

  private sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
  {
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
      => new TestAsyncEnumerable<TElement>(expression);

    public object? Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
      var resultType = typeof(TResult);
      if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
      {
        var innerResult = Execute(expression);
        return (TResult)typeof(Task)
          .GetMethod(nameof(Task.FromResult))!
          .MakeGenericMethod(resultType.GetGenericArguments()[0])
          .Invoke(null, [innerResult])!;
      }

      return Execute<TResult>(expression);
    }
  }

  private sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
  {
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
      => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
  }

  private sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
  {
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
      _inner.Dispose();
      return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
  }
}
