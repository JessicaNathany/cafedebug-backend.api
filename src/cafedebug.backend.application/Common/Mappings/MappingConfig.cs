using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Common.Mappings;

public static class MappingConfig
{
    public static BannerResponse ToBanner(this Banner banner)
    {
        return new BannerResponse
        {
            Id = banner.Id,
            Name = banner.Name,
            UrlImage = banner.UrlImage,
            Url = banner.Url,
            StartDate = banner.StartDate.ToShortDateString(),
            EndDate = banner.EndDate.ToShortDateString(),
            UpdateDate = banner.UpdatedAt.ToString(),
            Active = banner.Active.ToString(),
            Order = banner.Order
        };
    }

    public static EpisodeResponse ToEpisode(this Episode episode)
    {
         return new EpisodeResponse
         {
             Id = episode.Id,
             Title = episode.Title,
             Description = episode.Description,
             ShortDescription = episode.ShortDescription,
             Url = episode.Url,
             ImageUrl = episode.ImageUrl,
             Tags = episode.Tags,
             PublishedAt = episode.PublishedAt,
             Active = episode.Active,
             Number = episode.Number,
             Category = episode.Category?.ToCategory(),
             CategoryId = episode.CategoryId,
             DurationInSeconds = episode.DurationInSeconds
         };
    }

    public static CategoryResponse ToCategory(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static TeamResponse ToTeam(this Team team)
    {
        throw new NotImplementedException();
    }

    public static UserAdminResponse ToUserAdmin(this UserAdmin user)
    {
        throw new NotImplementedException();
    }
}

