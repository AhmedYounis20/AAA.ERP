using Accounting.Application.Commands.Banks;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for bank GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBankQueries : ISubLedgerQueries<Bank, BankCreateCommand>;
