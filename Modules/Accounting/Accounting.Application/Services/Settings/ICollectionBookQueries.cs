using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for collection book GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICollectionBookQueries : ISettingQueries<CollectionBook>;
