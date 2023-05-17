using AddressBookService.Api.Contracts;
using AddressBookService.Api.Contracts.V1.Requests;
using AddressBookService.Api.Contracts.V1.Responses;
using AddressBookService.Application.Interfaces;
using AddressBookService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookService.Api.Controllers.V1;

[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAddressService _addressService;

    public AddressesController(IMapper mapper, IAddressService addressService)
    {
        _mapper = mapper;
        _addressService = addressService;
    }

    [HttpGet(ApiRoutes.Addresses.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<AddressResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAddresses()
    {
        try
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            var addressResponses = _mapper.Map<IEnumerable<AddressResponse>>(addresses);
            return Ok(addressResponses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet(ApiRoutes.Addresses.Get)]
    [ProducesResponseType(typeof(IEnumerable<AddressResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAddress([FromRoute] Guid addressId)
    {
        try
        {
            var address = await _addressService.GetAddressByIdAsync(addressId);

            if (address is null)
            {
                return NotFound();
            }

            var addressResponse = _mapper.Map<AddressResponse>(address);
            return Ok(addressResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost(ApiRoutes.Addresses.Create)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAddress([FromBody] AddressRegistrationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressRequest = _mapper.Map<Address>(request);

            var createdAddress = await _addressService.CreateAddressAsync(addressRequest);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" +
                              ApiRoutes.Addresses.Get.Replace("{addressId}", createdAddress.Id.ToString());

            var response = _mapper.Map<AddressResponse>(createdAddress);
            return Created(locationUri, response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut(ApiRoutes.Addresses.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid addressId, [FromBody] AddressUpdateRequest request)
    {
        try
        {
            var address = await _addressService.GetAddressByIdAsync(addressId);
            if (address is null)
            {
                return NotFound();
            }

            var updatedAddress = _mapper.Map(request, address);
            await _addressService.UpdateAddressAsync(updatedAddress);

            var addressBookResponse = _mapper.Map<AddressResponse>(updatedAddress);
            return Ok(addressBookResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete(ApiRoutes.Addresses.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAddress([FromRoute] Guid addressId)
    {
        var address = await _addressService.GetAddressByIdAsync(addressId);
        if (address is null)
        {
            return NotFound();
        }

        await _addressService.DeleteAddressAsync(addressId);
        return NoContent();
    }
}