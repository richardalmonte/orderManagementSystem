using System.ComponentModel.DataAnnotations;

namespace UserService.Contracts.V1.Requests;

public record UserRegistrationRequest
{
    [Required] public string Email { get; set; }
    public string Password { get; set; }
}