using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Commands.Banks;

public class BankUpdateCommand : BaseSubLeadgerUpdateCommand<Bank>
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
    public string? BankAddress { get; set; }
}
