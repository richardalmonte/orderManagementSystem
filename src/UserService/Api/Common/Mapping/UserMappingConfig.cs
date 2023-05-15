using UserService.Contracts.V1.Requests;

namespace UserService.Common.Mapping;

public class UserMappingConfig : AutoMapper.Profile
{
    public UserMappingConfig()
    {
        // CreateMap<UserRegistrationRequest, User>()
        //     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        //     .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
        //     .ReverseMap();
    }
}