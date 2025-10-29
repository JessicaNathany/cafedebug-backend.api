using Newtonsoft.Json;

namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public class TeamViewModel
{
    [JsonProperty("nome")]
    public string Name { get; set; }

    [JsonProperty("urlGithub")]
    public string UrlGitHub { get; set; }

    [JsonProperty("urlInstagram")]
    public string UrlInstagram { get; set; }

    [JsonProperty("urlLinkedin")]
    public string UrlLinkedin { get; set; }

    [JsonProperty("urlImagem")]
    public string UrlImage { get; set; }

    [JsonProperty("profissao")]
    public string Job { get; set; }
}