using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Services.SubLeadgers;

internal static class SubLedgerNextDefaultsHelper
{
    public static async Task<ApiResponse<TCreate>> GetNextSubLedgersAsync<TEntity, TCreate>(
        IChartOfAccountRepository chartOfAccountRepository,
        Guid chartAccountParentId,
        Guid? parentId)
        where TEntity : BaseSettingEntity
        where TCreate : BaseSubLeadgerCreateCommand<TEntity>, new()
    {
        var code = await chartOfAccountRepository.GenerateNewCodeForChild(chartAccountParentId);

        return new ApiResponse<TCreate>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new TCreate
            {
                Code = code,
                NodeType = NodeType.Domain,
                ParentId = parentId
            }
        };
    }
}
