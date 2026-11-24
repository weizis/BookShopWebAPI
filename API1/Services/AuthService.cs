
using API1.DTOs.Auth;
using API1.Exceptions;
using API1.Models;
using API1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API1.Services
{
    public class AuthService : IAuthService
    {
        private readonly BookShopContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(BookShopContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден.");
            }

            if (user.IsActive != true)
            {
                throw new BadRequestException("Аккаунт деактивирован.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            if (!isPasswordValid)
            {
                throw new BadRequestException("Неверный email или пароль.");
            }

            return GenerateToken(user);
        }

        private TokenDto GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Клиент"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expirationMinutes = double.Parse(jwtSettings["ExpirationInMinutes"]!);
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Email = user.Email,
                Role = user.Role?.RoleName ?? "Клиент",
                UserId = user.UserId
            };
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUser != null)
            {
                throw new ConflictException("Пользователь с таким email уже существует.");
            }

            var clientRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == "Клиент");

            if (clientRole == null)
            {
                throw new BadRequestException("Роль 'Клиент' не найдена в системе.");
            }

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RoleId = clientRole.RoleId,
                RegistrationDate = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            newUser.Role = clientRole;

            return GenerateToken(newUser);
        }
    }
}
