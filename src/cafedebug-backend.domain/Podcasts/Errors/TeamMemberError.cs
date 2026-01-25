using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Podcasts.Errors;

public static class TeamMemberError
{
    public static Error NotFound(int teamMemberId)
    {
        return new Error(ErrorType.ResourceNotFound,$"Team member with id {teamMemberId} not found");
    }
    
}