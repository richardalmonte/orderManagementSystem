using AddressBookService.Api.Contracts.V1.Requests;
using AddressBookService.Api.Contracts.V1.Responses;
using AddressBookService.Domain.Entities;

namespace AddressBookService.Api.Common.Mapping;

public class AddressMappingConfig : AutoMapper.Profile
{
    public AddressMappingConfig()
    {
        CreateMap<AddressRegistrationRequest, Address>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ReverseMap();

        CreateMap<AddressResponse, Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ReverseMap();

        CreateMap<AddressUpdateRequest, Address>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ReverseMap();
    }
}