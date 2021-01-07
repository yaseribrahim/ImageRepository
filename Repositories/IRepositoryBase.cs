using System;
using System.Collections;
using System.Linq.Expressions;

namespace ImageRepo.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity GetEntity(string id);
        IEnumerable GetEntities();
        IEnumerable GetEntities(Expression<Func<TEntity, bool>> expression);
    }
}