using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class CollectionBookReadService(ICollectionBookService collectionBookService) : ICollectionBookReadService
{
    public Task<ApiResponse<IEnumerable<CollectionBook>>> GetAll() => collectionBookService.ReadAll();
    public Task<ApiResponse<PaginatedResult<CollectionBook>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default)
        => collectionBookService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<CollectionBook>> GetById(Guid id) => collectionBookService.ReadById(id);
}
