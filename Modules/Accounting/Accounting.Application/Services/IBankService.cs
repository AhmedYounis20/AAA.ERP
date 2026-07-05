using Accounting.Application.Commands.Banks;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

public interface IBankService : IBaseSubLeadgerService<Bank, BankCreateCommand, BankUpdateCommand>;
