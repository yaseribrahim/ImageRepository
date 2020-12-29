using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using ImageRepo.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageRepo.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }
        public void Create(TEntity entity)
        {
            repositoryContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            repositoryContext.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            repositoryContext.Set<TEntity>().Remove(entity);
        }

        public IEnumerable GetEntities()
        {
            return repositoryContext.Set<TEntity>().AsNoTracking();
        }

        public IEnumerable GetEntities(Expression<Func<TEntity, bool>> expression)
        {
            return repositoryContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public TEntity GetEntity(string id)
        {
            return repositoryContext.Set<TEntity>().Find(id);
        }
    }
}