using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class FeedbackRepository : RepositoryBase<FeedBacks>, IFeedbackRepository
{
    public FeedbackRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}