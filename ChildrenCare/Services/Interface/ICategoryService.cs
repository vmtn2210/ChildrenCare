using ChildrenCare.Entities;

namespace ChildrenCare.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
    }
}
