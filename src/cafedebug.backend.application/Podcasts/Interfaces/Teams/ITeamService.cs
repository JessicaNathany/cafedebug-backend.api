using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.Interfaces.Teams;

public interface ITeamService 
{
    Task Save(Team team);
    Task Update(Team team);
    Task Delete(Guid code);
}