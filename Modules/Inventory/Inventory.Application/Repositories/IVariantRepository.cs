using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Dtos.Inventory;
using Shared.DTOs;
using System.Linq.Expressions;

namespace ERP.Application.Repositories.Inventory
{
    public interface IVariantRepository : IBaseRepository<Variant>
    {
        Task<IEnumerable<VariantDto>> GetDtos();
        Task<PaginatedResult<VariantDto>> GetPaginatedDtos(
            int pageNumber,
            int pageSize,
            Expression<Func<Variant, bool>>? filter = null,
            Expression<Func<Variant, object>>? orderBy = null,
            bool descending = false,
            CancellationToken cancellationToken = default);
        Task<VariantDto?> GetDtoById(Guid id);
        Task<IEnumerable<VariantDto>> GetVariantsByProductId(Guid productId);
        Task<IEnumerable<VariantDto>> GetVariantsWithAttributes();
        Task<VariantDto?> GetVariantWithAttributes(Guid id);
        Task<IEnumerable<VariantDto>> SearchVariants(string searchTerm);
        Task<List<Variant>> GetByProductIdAsync(Guid productId);
    }
}
