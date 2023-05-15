using System.ComponentModel.DataAnnotations;

namespace UserService.Contracts.V1.Requests;

public record UserUpdateRequest
{
    [Required] public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}