using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Commands.Subleadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerCreateCommand<TEntity> : BaseTreeSettingCreateCommand<ApiResponse<SubLeadgerBaseEntity<TEntity>>> where TEntity :BaseSettingEntity
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