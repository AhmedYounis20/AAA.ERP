using AAA.ERP.OutputDtos;
using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.BaseEntities;
using Shared.Responses;

namespace AAA.ERP.Services.Interfaces.SubLeadgers;

public interface IBaseSubLeadgerService<TEntity,TCreateCommand,in  TUpdateCommand>
    : IBaseTreeSettingService<TEntity,TCreateCommand,TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}