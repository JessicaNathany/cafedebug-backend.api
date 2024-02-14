using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface ITeamService 
    {
        Task Save(Team team);
        Task Update(Team team);
        Task Delete(Guid code);
    }
}
