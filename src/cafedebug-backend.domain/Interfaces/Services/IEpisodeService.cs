using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEpisodeService : IDisposable
    {
        Task Save(Episode episode);

        Task Update(Episode episode);

        Task Delete(Guid code);
    }
}
