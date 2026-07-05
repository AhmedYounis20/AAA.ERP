using Accounting.Application.Commands.Banks;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Services;

public interface IBankService : IBaseSubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>;
