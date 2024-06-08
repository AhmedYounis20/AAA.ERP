using Shared.BaseEntities;

namespace Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

public interface IBaseBussinessValidator<TEntity> where TEntity : BaseEntity
{
    public Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateCreateBussiness(TEntity inpuModel);
    public Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateUpdateBussiness(TEntity inpuModel);
}
