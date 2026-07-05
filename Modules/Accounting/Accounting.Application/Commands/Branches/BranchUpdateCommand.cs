using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Commands.Branches;

public class BranchUpdateCommand : BaseSubLeadgerUpdateCommand<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public AttachmentDto? Logo { get; set; }
}
