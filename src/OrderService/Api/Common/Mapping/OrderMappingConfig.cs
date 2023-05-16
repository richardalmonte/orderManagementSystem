using OrderService.Api.Contracts.V1.Requests;
using OrderService.Api.Contracts.V1.Responses;
using OrderService.Domain.Entities;

namespace OrderService.Api.Common.Mapping;

public class OrderMappingConfig : AutoMapper.Profile
{
    public OrderMappingConfig()
    {
        CreateMap<OrderRegistrationRequest, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ReverseMap();

        CreateMap<OrderResponse, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ReverseMap();

        CreateMap<OrderUpdateRequest, Order>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DeliveryAddressId, opt => opt.MapFrom(src => src.DeliveryAddressId))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ReverseMap();
    }
}