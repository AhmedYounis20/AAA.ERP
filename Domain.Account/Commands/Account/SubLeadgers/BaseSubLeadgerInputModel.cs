using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;
using Shared.BaseEntities;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.SubLeadgers;

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