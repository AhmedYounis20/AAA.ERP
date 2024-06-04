using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.InputModels.Subleadgers;

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