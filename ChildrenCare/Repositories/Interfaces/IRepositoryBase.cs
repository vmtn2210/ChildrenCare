using System.Linq.Expressions;

namespace ChildrenCare.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindAllAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleByConditionWithIncludeAsync(Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> include);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByConditionAsyncWithInclude(Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> include);
        Task<IEnumerable<T>> FindByConditionAsyncWithMultipleIncludes(Expression<Func<T, bool>> expression, params 
            Expression<Func<T, object>>[] includes);
        Task<T> CreateAsync(T entity);
        Task<T> CreateWithoutSaveAsync(T entity);
        Task<T> UpdateAsync(T entity, object key);
        Task<T> UpdateAsyncWithoutSave(T entity, object key);
        T CreateWithoutSave(T t);
        Task<int> DeleteAsync(T entity);
        void DeleteWithoutSave(T entity);

    }
}
