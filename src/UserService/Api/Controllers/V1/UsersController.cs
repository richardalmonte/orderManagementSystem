using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;
using UserService.Contracts;
using UserService.Contracts.V1.Requests;
using UserService.Contracts.V1.Responses;
using UserService.Domain.Entities;

namespace UserService.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UsersController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpPost(ApiRoutes.Users.Create)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRequest = _mapper.Map<User>(request);

            var createdUser = await _userService.CreateUserAsync(userRequest);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Users.Get.Replace("{userId}", createdUser.Id.ToString());

            var response = _mapper.Map<UserResponse>(createdUser);
            return Created(locationUri, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return Ok();
    }
}