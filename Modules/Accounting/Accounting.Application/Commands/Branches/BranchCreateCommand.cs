using ERP.BuildingBlocks.Application.DTOs;
using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Commands.Branches;

public class BranchCreateCommand : BaseSubLeadgerCreateCommand<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public AttachmentDto? Logo { get; set; }
}
