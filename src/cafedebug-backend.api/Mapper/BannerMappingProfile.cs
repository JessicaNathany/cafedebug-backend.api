using AutoMapper;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.api.Mapper
{
    public class BannerMappingProfile : Profile
    {
        public BannerMappingProfile()
        {
            CreateMap<List<Banner>, BannerResponse>();

            CreateMap<Banner, BannerResponse>();
        }   
    }
}
