using System.ComponentModel.DataAnnotations;

namespace UserService.Contracts.V1.Requests;

public record UserUpdateRequest
{
    [Required] public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}