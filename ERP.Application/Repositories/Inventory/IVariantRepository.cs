using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Repositories.Inventory
{
    public interface IVariantRepository : IBaseRepository<Variant>
    {
        Task<IEnumerable<VariantDto>> GetDtos();
        Task<VariantDto?> GetDtoById(Guid id);
        Task<IEnumerable<VariantDto>> GetVariantsByProductId(Guid productId);
        Task<IEnumerable<VariantDto>> GetVariantsWithAttributes();
        Task<VariantDto?> GetVariantWithAttributes(Guid id);
        Task<IEnumerable<VariantDto>> SearchVariants(string searchTerm);
        Task<List<Variant>> GetByProductIdAsync(Guid productId);
    }
}
