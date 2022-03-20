using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Domain.DTOs.Authentication;

public class AuthenticateRequest
{
    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;
}