using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Episodes.Errors;

public static class EpisodeError
{
    public static Error AlreadyExists(string message)
    {
       return  new Error(ErrorType.ExistingRegister, $"Already exists an episode with the title {message}");
    }

    public static Error NotFound(int episodeId)
    {
        return new Error(ErrorType.ResourceNotFound, $"Episode with id {episodeId} not found");
    }
}