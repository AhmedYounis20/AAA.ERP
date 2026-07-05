using System.Net;
using Accounting.Contracts.Integration;
using FluentAssertions;
using Moq;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.UnitTests.Contracts;

/// <summary>
/// Mock-based contract tests for <see cref="IAccountingIntegrationService"/>.
/// Documents idempotent GL posting: duplicate calls with the same
/// <c>(SourceDocumentType, SourceDocumentId)</c> must return the same entry id without creating a second post.
/// Production behavior: <c>AccountingIntegrationService.TryGetExistingPostedEntryAsync</c>.
/// </summary>
public class AccountingIntegrationServiceContractTests
{
  [Fact]
  public async Task PostInventoryTransactionEntry_duplicate_SourceDocumentId_returns_same_entry_id()
  {
    var sourceDocumentId = Guid.NewGuid();
    var entryId = Guid.NewGuid();
    var callCount = 0;

    var mock = new Mock<IAccountingIntegrationService>();
    mock.Setup(s => s.PostInventoryTransactionEntryAsync(
        It.Is<InventoryTransactionPostingRequest>(r => r.SourceDocumentId == sourceDocumentId),
        It.IsAny<CancellationToken>()))
      .ReturnsAsync(() =>
      {
        callCount++;
        return new ApiResponse<Guid>
        {
          IsSuccess = true,
          StatusCode = HttpStatusCode.OK,
          Result = entryId
        };
      });

    var request = CreateTransactionRequest(sourceDocumentId, InventoryGlDirection.Receipt);
    var first = await mock.Object.PostInventoryTransactionEntryAsync(request);
    var second = await mock.Object.PostInventoryTransactionEntryAsync(request);

    first.IsSuccess.Should().BeTrue();
    second.IsSuccess.Should().BeTrue();
    second.Result.Should().Be(first.Result);
    callCount.Should().Be(2, "callers may retry; contract allows duplicate invocations when implementation is idempotent");
  }

  [Fact]
  public async Task Idempotent_fake_second_post_returns_first_entry_without_duplicate_key()
  {
    var fake = new IdempotentAccountingIntegrationFake();
    var sourceDocumentId = Guid.NewGuid();
    var request = CreateTransactionRequest(sourceDocumentId, InventoryGlDirection.Issue);

    var first = await fake.PostInventoryTransactionEntryAsync(request);
    var second = await fake.PostInventoryTransactionEntryAsync(request);

    first.IsSuccess.Should().BeTrue();
    second.IsSuccess.Should().BeTrue();
    second.Result.Should().Be(first.Result);
    fake.PostCount.Should().Be(1, "idempotent implementation must not create a second GL entry for the same SourceDocumentId");
  }

  [Fact]
  public async Task PostInventoryTransferEntry_uses_source_document_type_and_id_as_idempotency_key()
  {
    var fake = new IdempotentAccountingIntegrationFake();
    var sourceDocumentId = Guid.NewGuid();
    var request = new InventoryTransferPostingRequest(
      sourceDocumentId,
      EntrySourceDocumentTypes.InventoryTransferStage,
      Guid.NewGuid(),
      Guid.NewGuid(),
      Guid.NewGuid(),
      DateTime.UtcNow,
      Guid.NewGuid(),
      100m);

    var first = await fake.PostInventoryTransferEntryAsync(request);
    var second = await fake.PostInventoryTransferEntryAsync(request with { TotalAmount = 200m });

    first.Result.Should().Be(second.Result);
    fake.TransferPostCount.Should().Be(1);
  }

  [Fact]
  public async Task PostInventoryTransactionEntry_different_SourceDocumentId_creates_distinct_entries()
  {
    var fake = new IdempotentAccountingIntegrationFake();
    var firstId = Guid.NewGuid();
    var secondId = Guid.NewGuid();

    var first = await fake.PostInventoryTransactionEntryAsync(
      CreateTransactionRequest(firstId, InventoryGlDirection.Receipt));
    var second = await fake.PostInventoryTransactionEntryAsync(
      CreateTransactionRequest(secondId, InventoryGlDirection.Receipt));

    first.Result.Should().NotBe(second.Result);
    fake.PostCount.Should().Be(2);
  }

  [Theory]
  [InlineData(InventoryGlDirection.Receipt)]
  [InlineData(InventoryGlDirection.Issue)]
  public async Task PostInventoryTransactionEntry_mock_verifies_direction_on_request(InventoryGlDirection direction)
  {
    var mock = new Mock<IAccountingIntegrationService>();
  InventoryTransactionPostingRequest? captured = null;

    mock.Setup(s => s.PostInventoryTransactionEntryAsync(
        It.IsAny<InventoryTransactionPostingRequest>(),
        It.IsAny<CancellationToken>()))
      .Callback<InventoryTransactionPostingRequest, CancellationToken>((req, _) => captured = req)
      .ReturnsAsync(new ApiResponse<Guid>
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK,
        Result = Guid.NewGuid()
      });

    var request = CreateTransactionRequest(Guid.NewGuid(), direction);
    await mock.Object.PostInventoryTransactionEntryAsync(request);

    captured.Should().NotBeNull();
    captured!.Direction.Should().Be(direction);
    captured.SourceDocumentId.Should().Be(request.SourceDocumentId);
  }

  [Fact]
  public void EntrySourceDocumentTypes_inventory_values_are_stable()
  {
    EntrySourceDocumentTypes.InventoryTransaction.Should().Be("InventoryTransaction");
    EntrySourceDocumentTypes.InventoryTransferStage.Should().Be("InventoryTransferStage");
    EntrySourceDocumentTypes.InventoryTransferComplete.Should().Be("InventoryTransferComplete");
    EntrySourceDocumentTypes.InventoryTransferReject.Should().Be("InventoryTransferReject");
  }

  private static InventoryTransactionPostingRequest CreateTransactionRequest(
    Guid sourceDocumentId,
    InventoryGlDirection direction)
    => new(
      sourceDocumentId,
      Guid.NewGuid(),
      Guid.NewGuid(),
      DateTime.UtcNow,
      Guid.NewGuid(),
      50m,
      direction);

  /// <summary>
  /// Minimal in-memory fake documenting the idempotency contract for inventory GL posts.
  /// </summary>
  private sealed class IdempotentAccountingIntegrationFake : IAccountingIntegrationService
  {
    private readonly Dictionary<(string Type, Guid Id), Guid> _postedEntries = new();

    public int PostCount { get; private set; }

    public int TransferPostCount { get; private set; }

    public Task<ApiResponse<FinancialPeriodInfo>> GetOpenPeriodForDateAsync(
      DateTime date,
      CancellationToken cancellationToken = default)
      => Task.FromResult(new ApiResponse<FinancialPeriodInfo> { IsSuccess = false });

    public Task<ApiResponse<BranchAccountInfo>> GetBranchAccountAsync(
      Guid branchId,
      CancellationToken cancellationToken = default)
      => Task.FromResult(new ApiResponse<BranchAccountInfo> { IsSuccess = false });

    public Task<ApiResponse<Guid>> ValidatePartyAccountAsync(
      Guid chartOfAccountId,
      CancellationToken cancellationToken = default)
      => Task.FromResult(new ApiResponse<Guid> { IsSuccess = false });

    public Task<ApiResponse<Guid>> PostInventoryTransactionEntryAsync(
      InventoryTransactionPostingRequest request,
      CancellationToken cancellationToken = default)
    {
      var key = (EntrySourceDocumentTypes.InventoryTransaction, request.SourceDocumentId);
      if (_postedEntries.TryGetValue(key, out var existing))
      {
        return Task.FromResult(Success(existing));
      }

      PostCount++;
      var entryId = Guid.NewGuid();
      _postedEntries[key] = entryId;
      return Task.FromResult(Success(entryId));
    }

    public Task<ApiResponse<Guid>> PostInventoryTransferEntryAsync(
      InventoryTransferPostingRequest request,
      CancellationToken cancellationToken = default)
    {
      var key = (request.SourceDocumentType, request.SourceDocumentId);
      if (_postedEntries.TryGetValue(key, out var existing))
      {
        return Task.FromResult(Success(existing));
      }

      TransferPostCount++;
      var entryId = Guid.NewGuid();
      _postedEntries[key] = entryId;
      return Task.FromResult(Success(entryId));
    }

    private static ApiResponse<Guid> Success(Guid entryId)
      => new()
      {
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK,
        Result = entryId
      };
  }
}
