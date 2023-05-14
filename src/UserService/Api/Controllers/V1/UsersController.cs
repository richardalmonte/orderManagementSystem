using Microsoft.AspNetCore.Mvc;
using UserService.Contracts;
using UserService.Contracts.V1.Requests;

namespace UserService.Controllers.V1;

public class UsersController : Controller
{
    public UsersController()
    {
    }

    [HttpPost(ApiRoutes.Users.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(UserRegistrationRequest user)
    {
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return Ok();
    }
}