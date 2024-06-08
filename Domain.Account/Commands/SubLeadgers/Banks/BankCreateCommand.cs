using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.SubLeadgers;

namespace Domain.Account.Commands.SubLeadgers.Banks;

public class BankCreateCommand : BaseSubLeadgerCreateCommand<Bank>
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
    public string? BankAddress { get; set; }
}