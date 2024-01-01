using AutoMapper;

namespace TermProject.models;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<MyUser, UserVm>();
    }
    
}