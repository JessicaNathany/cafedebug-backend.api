using AutoMapper;
using cafedebug_backend.application.Request;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.api.Mapper
{
    public class BannerMappingProfile : Profile
    {
        public BannerMappingProfile()
        {
            CreateMap<Banner, BannerResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImage))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd") : null))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active.ToString()));

            CreateMap<BannerRequest, Banner>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImage))
               .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.DateUpdate))
               .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active.ToString()));
        }   
    }
}
