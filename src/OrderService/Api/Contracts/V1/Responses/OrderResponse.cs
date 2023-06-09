﻿namespace OrderService.Api.Contracts.V1.Responses;

public record OrderResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; }
}