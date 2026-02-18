using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthController(JwtService jwtService, IConfiguration config)
    {
        _jwtService = jwtService;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var username = _config["DemoAuth:Username"];
        var password = _config["DemoAuth:Password"];

        if (request.Username == username && request.Password == password)
        {
            var token = _jwtService.GenerateToken(request.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}
