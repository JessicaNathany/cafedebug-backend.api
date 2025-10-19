using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Episodes.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task Save(Category category);

        Task Update(Category category);

        Task Delete(Guid code);
    }
}
