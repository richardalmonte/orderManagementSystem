using ProductService.API.Contracts.V1.Requests;
using ProductService.API.Contracts.V1.Responses;
using ProductService.Domain.Entities;

namespace ProductService.API.Common.Mapping;

public class CategoryMappingConfig : AutoMapper.Profile
{
    public CategoryMappingConfig()
    {
        CreateMap<CategoryRegistrationRequest, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<CategoryResponse, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<CategoryUpdateRequest, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
    }
}