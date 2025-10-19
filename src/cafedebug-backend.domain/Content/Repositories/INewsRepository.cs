using cafedebug_backend.domain.Content;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
    }
}
