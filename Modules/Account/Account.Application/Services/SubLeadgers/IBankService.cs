using Account.Domain.Commands.SubLeadgers.Banks;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Services.SubLeadgers;

public interface IBankService : IBaseSubLeadgerService<Bank,BankCreateCommand,BankUpdateCommand>
{ }