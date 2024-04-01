using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Responses;

namespace AAA.ERP.Services.BaseServices.interfaces
{
    public interface IBaseTreeService<TEntity> : IBaseService<TEntity> where TEntity : BaseTreeEntity
    {}
}
