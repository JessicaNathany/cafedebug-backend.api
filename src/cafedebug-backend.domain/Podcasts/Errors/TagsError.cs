using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Podcasts.Errors
{
    public class TagsError
    {
        public static Error AlreadyExists(string message)
        {
            return new Error(ErrorType.ExistingRegister, $"Already exists an tags with the name {message}");
        }

        public static Error NotFound(int tagsId)
        {
            return new Error(ErrorType.ResourceNotFound, $"Tags with id {tagsId} not found");
        }
    }
}
