using AutoMapper;

namespace TermProject.models.WatchlistModels;

public class WatchlistMappingProfile : Profile
{
    public WatchlistMappingProfile()
    {
        CreateMap<UserWatchlist, WatchlistVm>()
            .ForMember(dest => dest.Code, opt 
                => opt.MapFrom(src => src.Stock.Code))
            .ForMember(dest => dest.Name, opt 
                => opt.MapFrom(src => src.Stock.Name))
            .ForMember(dest => dest.Type, opt 
                => opt.MapFrom(src => src.Stock.Type));
    }
}