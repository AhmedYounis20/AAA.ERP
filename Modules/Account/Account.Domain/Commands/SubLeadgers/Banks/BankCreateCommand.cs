using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.Banks;

public class BankCreateCommand : BaseSubLeadgerCreateCommand<Bank>
{
    public string? BankAccount { get; set; }
    public string? BankAddress { get; set; }
}