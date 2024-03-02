using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Validators.BussinessValidator.Interfaces;

public interface IBaseBussinessValidator<TEntity> where TEntity : BaseEntity
{
    public Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateCreateBussiness(TEntity inpuModel);
    public Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateUpdateBussiness(TEntity inpuModel);
}
