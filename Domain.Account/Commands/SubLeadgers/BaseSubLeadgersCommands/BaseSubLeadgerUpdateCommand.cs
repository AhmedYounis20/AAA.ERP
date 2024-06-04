using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Commands.Subleadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerCreateCommand<TEntity> : BaseTreeSettingCreateCommand<ApiResponse<SubLeadgerBaseEntity<TEntity>>> where TEntity :BaseSettingEntity
{
    public NodeType NodeType { get; set; }
    public string? Code { get; set; }  
}