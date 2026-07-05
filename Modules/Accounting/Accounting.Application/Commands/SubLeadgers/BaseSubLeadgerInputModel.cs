using Accounting.Domain.Models.Entities.SubLeadgers;
using Shared;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Commands.SubLeadgers;

public class BaseSubLeadgerInputModel : ICommand<ApiResponse<CashInBox>>
{
    public Guid Id { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public Guid? ParentId { get; set; }
    public string? Notes { get; set; }
    public NodeType NodeType { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}