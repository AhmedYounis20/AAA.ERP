using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Services;

public class SupplierService(IChartOfAccountRepository chartOfAccountRepository) : ISupplierService
{
    public async Task<ApiResponse<SupplierCreateCommand>> GetNextSubLeadgers(Guid? parentId)
    {
        var code = await chartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(SD.SupplierChartOfAccountId));

        return new ApiResponse<SupplierCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new SupplierCreateCommand
            {
                Code = code,
                NodeType = NodeType.Domain,
                ParentId = parentId
            }
        };
    }
}
