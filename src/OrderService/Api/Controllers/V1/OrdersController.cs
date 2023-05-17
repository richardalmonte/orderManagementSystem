using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Contracts;
using OrderService.Api.Contracts.V1.Requests;
using OrderService.Api.Contracts.V1.Responses;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Api.Controllers.V1;

[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public OrdersController(IMapper mapper, IOrderService orderService)
    {
        _mapper = mapper;
        _orderService = orderService;
    }

    [HttpGet(ApiRoutes.Orders.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return Ok(orderResponses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet(ApiRoutes.Orders.Get)]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrder([FromRoute] Guid orderId)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order is null)
            {
                return NotFound();
            }

            var orderResponse = _mapper.Map<OrderResponse>(order);
            return Ok(orderResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost(ApiRoutes.Orders.Create)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRegistrationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var orderRequest = _mapper.Map<Order>(request);

            var createdOrder = await _orderService.CreateOrderAsync(orderRequest);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Orders.Get.Replace("{orderId}", createdOrder.Id.ToString());

            var response = _mapper.Map<OrderResponse>(createdOrder);
            return Created(locationUri, response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut(ApiRoutes.Orders.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid orderId, [FromBody] OrderUpdateRequest request)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return NotFound();
            }

            var updatedOrder = _mapper.Map(request, order);
            await _orderService.UpdateOrderAsync(updatedOrder);

            var orderResponse = _mapper.Map<OrderResponse>(updatedOrder);
            return Ok(orderResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete(ApiRoutes.Orders.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order is null)
        {
            return NotFound();
        }

        await _orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }
}