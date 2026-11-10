
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarksPracticeApi.Data;
using MarksPracticeApi.DTOs.Auth;
using MarksPracticeApi.Models;
using MarksPracticeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MarksPracticeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly IConfiguration _configuration;

    public AuthController(
        AppDbContext context,
        PasswordService passwordService,
        IConfiguration configuration)
    {
        _context = context;
        _passwordService = passwordService;
        _configuration = configuration;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser = await _context.Users
            .AnyAsync(u => u.Email == email);

        if (existingUser)
        {
            return Conflict(new
            {
                message = "Email is already registered."
            });
        }

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Role = "Student",
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Store hashed password, never plain password
        user.PasswordHash = _passwordService.HashPassword(
            user,
            dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return StatusCode(201, new
        {
            message = "User registered successfully.",
            userId = user.Id,
            email = user.Email
        });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !user.IsActive)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        // Verify entered password against stored hash
        var isPasswordValid = _passwordService.VerifyPassword(
            user,
            dto.Password);

        if (!isPasswordValid)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var token = GenerateJwtToken(user);

        var response = new AuthResponseDto
        {
            Token = token.Token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            ExpiresAtUtc = token.ExpiresAtUtc
        };

        return Ok(response);
    }

    private (string Token, DateTime ExpiresAtUtc) GenerateJwtToken(User user)
    {
        var keyValue = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is missing.");

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer is missing.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience is missing.");

        var key = new SymmetricSecurityKey(
            Convert.FromBase64String(keyValue));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(60);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return (token, expiresAtUtc);
    }
}
