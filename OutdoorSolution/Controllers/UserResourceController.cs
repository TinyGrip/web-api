using OutdoorSolution.Filters;

namespace OutdoorSolution.Controllers
{
    [InitUserId]
    public abstract class UserResourceController : PagingController
    {
        public abstract void InitUser(string userId);
    }
}
