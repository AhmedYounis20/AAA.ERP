using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;

namespace Domain.Account.Commands.SubLeadgers.CashInBoxes;

public class BranchCreateCommand : BaseSubLeadgerCreateCommand<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public IFormFile? Logo { get; set; }
    
}