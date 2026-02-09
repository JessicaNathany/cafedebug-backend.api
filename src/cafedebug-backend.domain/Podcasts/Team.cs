using cafedebug_backend.domain.Shared;
namespace cafedebug_backend.domain.Podcasts;
public class Team : Entity
{
    public string Name { get; private set; }
    public string UrlGitHub { get; private set; }
    public string UrlInstagram { get; private set; }
    public string UrlLinkedin { get; private set; }
    public string UrlImage { get; private set; }
    public string Job { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
}