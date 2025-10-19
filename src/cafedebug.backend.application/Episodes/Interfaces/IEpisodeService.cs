using cafedebug_backend.domain.Episodes.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Episodes.Interfaces;

public interface IEpisodeService
{
    Task<Result<Episode>> UpdateAsync(Episode episodeRequest);

    Task<Result> DeleteAsync(int id);

    Task<Result<Episode>> GetByIdAsync(int id);

    Task<Result<List<Episode>>> GetAllAsync();
}