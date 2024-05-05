﻿using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Services.BaseServices.interfaces;

public interface IBaseTreeService<TEntity> : IBaseService<TEntity> where TEntity : BaseTreeEntity<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
}