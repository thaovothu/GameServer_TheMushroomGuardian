using GameServer.DTOs.Auth;

namespace GameServer.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> GuestLoginAsync(GuestLoginRequest request);
}
