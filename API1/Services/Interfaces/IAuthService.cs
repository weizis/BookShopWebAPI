
using API1.DTOs.Auth;
using API1.DTOs.User;

namespace API1.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> LoginAsync(LoginDto loginDto);
    }
}
