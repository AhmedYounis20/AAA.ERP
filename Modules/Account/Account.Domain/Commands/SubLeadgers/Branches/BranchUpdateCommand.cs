using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.Branches;

public class BranchUpdateCommand : BaseSubLeadgerUpdateCommand<Branch>
{
    public AttachmentDto? Logo { get; set; }
}