using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface IBankService : IBaseSubLeadgerService<Bank,BankCreateCommand,BankUpdateCommand>
{ }