using Accounting.Application.Commands.Customers;
using Accounting.Application.Services.SubLedger;
using Accounting.Application.Services.SubLedger.Base;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for customer GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICustomerQueries : ISubLedgerQueries<Customer, CustomerCreateCommand>;
