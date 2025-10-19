using AutoMapper;
using cafedebug_backend.domain.Banners;
using cafedebug.backend.application.Banners.DTOs.Responses;

namespace cafedebug_backend.api.Mapper;

public class BannerMappingProfile : Profile
{
    public BannerMappingProfile()
    {
        CreateMap<List<Banner>, BannerResponse>();

        CreateMap<Banner, BannerResponse>();
    }   
}