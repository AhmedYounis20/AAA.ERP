using Accounting.Application.Commands.Banks;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for bank.
/// </summary>
public interface IBankService : IBaseSubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>;
