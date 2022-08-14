using System.Linq.Expressions;
using ChildrenCare.Data;
using ChildrenCare.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public ChildrenCareDBContext CDBContext { get; set; }
        protected RepositoryBase(ChildrenCareDBContext cdbContext)
        {
            CDBContext = cdbContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await CDBContext.Set<T>().ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await CDBContext.Set<T>().AnyAsync(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await CDBContext.Set<T>().CountAsync(expression);
        }

        public async Task<T?> FindSingleByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await CDBContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<T?> FindSingleByConditionWithIncludeAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>> include)
        {
            return await CDBContext.Set<T>().Include(include).FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await CDBContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAsyncWithInclude(Expression<Func<T, bool>> expression, Expression<Func<T, object>> include)
        {
            return await CDBContext.Set<T>().Where(expression).Include(include).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAsyncWithMultipleIncludes(Expression<Func<T, bool>> expression,
            params Expression<Func<T, object>>[] includes)
        {
            var query = CDBContext.Set<T>().Where(expression);
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await CDBContext.Set<T>().AddAsync(entity);
            await CDBContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> CreateWithoutSaveAsync(T entity)
        {
            await CDBContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public T CreateWithoutSave(T t)
        {
            CDBContext.Set<T>().Add(t);
            return t;
        }

        public async Task<T> UpdateAsync(T entity, object key)
        {
            if (entity == null)
                return null;
            var exist = await CDBContext.Set<T>().FindAsync(key);
            if (exist == null) return null;
            CDBContext.Entry(entity).CurrentValues.SetValues(entity);
            await CDBContext.SaveChangesAsync();
            return exist;
        }

        public async Task<T> UpdateAsyncWithoutSave(T entity, object key)
        {
            if (entity == null)
                return null;
            var exist = await CDBContext.Set<T>().FindAsync(key);
            if (exist == null) return null;
            CDBContext.Entry(exist).CurrentValues.SetValues(entity);
            return exist;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            CDBContext.Set<T>().Remove(entity);
            return await CDBContext.SaveChangesAsync();
        }

        public void DeleteWithoutSave(T entity)
        {
            CDBContext.Set<T>().Remove(entity);
        }
    }
}
