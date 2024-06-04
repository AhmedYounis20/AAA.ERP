using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<ApiResponse<SubLeadgerBaseEntity<TEntity>>> where TEntity :BaseSettingEntity
{
    public NodeType NodeType { get; set; }
    public string? Code { get; set; }  
}