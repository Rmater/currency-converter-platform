using Microsoft.AspNetCore.Authorization;
using CurrencyConverter.Api.Auth;
using CurrencyConverter.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace CurrencyConverter.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(JwtTokenGenerator tokenGenerator) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var role = request.Username.ToLowerInvariant() switch
        {
            "admin" when request.Password == "Admin123!" => "admin",
            "viewer" when request.Password == "Viewer123!" => "viewer",
            _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(role))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        return Ok(new { accessToken = tokenGenerator.Generate(request.Username, role), role });
    }
}
