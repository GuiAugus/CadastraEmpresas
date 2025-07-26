using CadastraEmpresas.Data;
using CadastraEmpresas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace CadastraEmpresas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                return BadRequest(new { message = "E-mail ja cadastrado." });
            }

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado com sucesso!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == login.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.SenhaHash))
            {
                return Unauthorized(new { message = "E-mail ou senha invalidos." });
            }

            var token = GerarToken(usuario);

            return Ok(new { token });
        }

        private string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}