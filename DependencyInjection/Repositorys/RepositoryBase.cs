using Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositorys
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        protected RepositoryContext RepositoryContext;
        public RepositoryBase(RepositoryContext context) => RepositoryContext = context;
        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ?
            RepositoryContext.Set<T>().AsNoTracking()
            : RepositoryContext.Set<T>();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
             => RepositoryContext.Set<T>(); 
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        public void Delete(T entity)  => RepositoryContext.Set<T>().Remove(entity);
        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    }
}