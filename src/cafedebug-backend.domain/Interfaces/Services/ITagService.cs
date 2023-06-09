using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface ITagService : IDisposable
    {
        Task Save(Tag category);

        Task Update(Tag category);

        Task Delete(Guid code);
    }
}
