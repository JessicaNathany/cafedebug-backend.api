using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IBannerService : IDisposable
    {
        Task Save(Banner banner);
        Task Update(Banner banner);
        Task Delete(Guid code);
    }
}
