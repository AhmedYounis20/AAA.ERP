using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;

namespace Domain.Account.Commands.SubLeadgers.CashInBoxes;

public class BranchUpdateCommand : BaseSubLeadgerUpdateCommand<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public AttachmentDto? Logo { get; set; }
}