using AddressBookService.Api.Contracts.V1.Requests;
using AddressBookService.Api.Contracts.V1.Responses;
using AddressBookService.Domain.Entities;

namespace AddressBookService.Api.Common.Mapping;

public class AddressItemMappingConfig : AutoMapper.Profile
{
    public AddressItemMappingConfig()
    {
        CreateMap<AddressItemRegistrationRequest, AddressItem>()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();

        CreateMap<AddressItemResponse, AddressItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();

        CreateMap<AddressItemUpdateRequest, AddressItem>()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();
    }
}