using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class SliderRepository : RepositoryBase<Slider>, ISliderRepository
{
    public SliderRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}