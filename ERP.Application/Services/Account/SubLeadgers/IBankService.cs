using ERP.Domain.Commands.Account.SubLeadgers.Banks;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface IBankService : IBaseSubLeadgerService<Bank,BankCreateCommand,BankUpdateCommand>
{ }