using Accounting.Application.Commands.Banks;
using Accounting.Application.Services.SubLedger.Base;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for bank GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBankQueries : ISubLedgerQueries<Bank, BankCreateCommand>;
