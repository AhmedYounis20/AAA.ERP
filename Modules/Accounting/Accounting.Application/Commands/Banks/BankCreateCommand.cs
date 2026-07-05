using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Commands.Banks;

public class BankCreateCommand : BaseSubLeadgerCreateCommand<Bank>
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
    public string? BankAddress { get; set; }
}
