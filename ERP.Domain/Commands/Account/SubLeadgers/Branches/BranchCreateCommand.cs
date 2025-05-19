using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;

namespace ERP.Domain.Commands.Account.SubLeadgers.Branches;

public class BranchCreateCommand : BaseSubLeadgerCreateCommand<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public AttachmentDto? Logo { get; set; }

}