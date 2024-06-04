using AAA.ERP.OutputDtos;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace AAA.ERP.Services.Interfaces.SubLeadgers;

public interface ICashInBoxService
{
    Task<ApiResponse<CashInBox>> Get(Guid id);
    Task<ApiResponse<IEnumerable<CashInBox>>> Get();
    Task<ApiResponse<BaseSubLeadgerInputModel>> GetNextSubLeadgers(Guid? parentId);
    Task<ApiResponse<CashInBox>> Create(BaseSubLeadgerInputModel inputModel);
    Task<ApiResponse<CashInBox>> Update(BaseSubLeadgerInputModel inputModel);
    Task<ApiResponse<CashInBox>> Delete(Guid id);
}