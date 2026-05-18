using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Simulador_Mario_Kart.Data;
using Simulador_Mario_Kart.DTOs;
using Simulador_Mario_Kart.Models;


namespace Simulador_Mario_Kart.Controller
{
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class AuthController : ControllerBase
        {
            private readonly AppDbContext _db;
            private readonly IConfiguration _config;

            public AuthController(AppDbContext db, IConfiguration config)
            {
                _db = db;
                _config = config;
            }

            /// <summary>Registra um novo usuário no sistema.</summary>
            [HttpPost("register")]
            [ProducesResponseType(typeof(AuthResponse), 201)]
            [ProducesResponseType(400)]
            public async Task<IActionResult> Register([FromBody] RegisterRequest req)
            {
                if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                    return BadRequest(new { message = "Email já cadastrado." });

                if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                    return BadRequest(new { message = "Username já em uso." });

                var user = new User
                {
                    Username = req.Username,
                    Email = req.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(Register), GenerateToken(user));
            }

            /// <summary>Autentica o usuário e retorna um JWT.</summary>
            [HttpPost("login")]
            [ProducesResponseType(typeof(AuthResponse), 200)]
            [ProducesResponseType(401)]
            public async Task<IActionResult> Login([FromBody] LoginRequest req)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

                if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                    return Unauthorized(new { message = "Credenciais inválidas." });

                return Ok(GenerateToken(user));
            }

            private AuthResponse GenerateToken(User user)
            {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: creds
                );

                return new AuthResponse(
                    new JwtSecurityTokenHandler().WriteToken(token),
                    user.Username,
                    user.Email,
                    user.Id
                );
            }
        }
    }

