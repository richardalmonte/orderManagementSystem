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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
            return Ok(userResponses);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost(ApiRoutes.Users.Create)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserUpdateRequest request)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var updatedUser = _mapper.Map(request, user);
            await _userService.UpdateUserAsync(updatedUser);

            var userResponse = _mapper.Map<UserResponse>(updatedUser);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}