using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEpisodeService
    {
        Task<Result<Episode>> CreateAsync(Episode episodeRequest);

        Task<Result<Episode>> UpdateAsync(Episode episodeRequest);

        Task<Result> DeleteAsync(int id);

        Task<Result<Episode>> GetByIdAsync(int id);

        Task<Result<List<Episode>>> GetAllAsync();
    }
}
