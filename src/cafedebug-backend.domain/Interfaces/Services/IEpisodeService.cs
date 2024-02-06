using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEpisodeService 
    {
        Task<Result<Episode>> CreateAsync(Episode episodeRequest, CancellationToken cancellationToken);

        Task<Result<Episode>> UpdateAsync(Episode episodeRequest, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<Result<Episode>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Result<List<Episode>>> GetAllAsync();
    }
}
