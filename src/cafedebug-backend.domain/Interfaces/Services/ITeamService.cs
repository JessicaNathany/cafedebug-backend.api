using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface ITeamService : IDisposable
    {
        Task Save(Team team);
        Task Update(Team team);
        Task Delete(Guid code);
    }
}
