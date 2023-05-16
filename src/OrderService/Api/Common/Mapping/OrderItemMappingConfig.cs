using OrderService.Contracts.V1.Requests;
using OrderService.Contracts.V1.Responses;
using OrderService.Domain.Entities;

namespace OrderService.Common.Mapping;

public class OrderItemMappingConfig : AutoMapper.Profile
{
    public OrderItemMappingConfig()
    {
        CreateMap<OrderItemRegistrationRequest, OrderItem>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();

        CreateMap<OrderItemResponse, OrderItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();

        CreateMap<OrderItemUpdateRequest, OrderItem>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ReverseMap();
    }
}