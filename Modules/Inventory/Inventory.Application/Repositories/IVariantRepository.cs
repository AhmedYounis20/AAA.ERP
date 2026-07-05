using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Dtos;
using Shared.DTOs;
using System.Linq.Expressions;

namespace Inventory.Application.Repositories
{
    /// <summary>
    /// Repository contract for variant persistence.
    /// </summary>
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
        /// <summary>Gets dto by id.</summary>
        Task<VariantDto?> GetDtoById(Guid id);
        Task<IEnumerable<VariantDto>> GetVariantsByProductId(Guid productId);
        Task<IEnumerable<VariantDto>> GetVariantsWithAttributes();
        /// <summary>Gets variant with attributes.</summary>
        Task<VariantDto?> GetVariantWithAttributes(Guid id);
        Task<IEnumerable<VariantDto>> SearchVariants(string searchTerm);
        Task<List<Variant>> GetByProductIdAsync(Guid productId);
    }
}
