using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.Branches;

public class BranchCreateCommand : BaseSubLeadgerCreateCommand<Branch>
{
    public AttachmentDto? Logo { get; set; }
}