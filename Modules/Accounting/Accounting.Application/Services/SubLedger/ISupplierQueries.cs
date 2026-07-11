using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services.SubLedger;
using Accounting.Application.Services.SubLedger.Base;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for supplier GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISupplierQueries : ISubLedgerQueries<Supplier, SupplierCreateCommand>;
