using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;

namespace ChildrenCare.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CategoryService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _repositoryWrapper.Category.FindAllAsync();
    }
}