using GameServer.DTOs.Auth;
using GameServer.Models;
using GameServer.Repositories;

namespace GameServer.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPlayerRepository _playerRepo;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepo, IPlayerRepository playerRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _playerRepo = playerRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already in use");

        if (await _userRepo.UsernameExistsAsync(request.Username))
            throw new InvalidOperationException("Username already taken");

        var user = new User
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepo.CreateAsync(user);

        var player = new Player { UserId = user.Id };
        await _playerRepo.CreateAsync(player);

        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            PlayerId = user.Id.ToString(),
            DisplayName = user.Username
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        User? user;

        if (request.EmailOrUsername.Contains('@'))
            user = await _userRepo.GetByEmailAsync(request.EmailOrUsername);
        else
            user = await _userRepo.GetByUsernameAsync(request.EmailOrUsername);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            PlayerId = user.Id.ToString(),
            DisplayName = user.Username
        };
    }

    public async Task<AuthResponse> GuestLoginAsync(GuestLoginRequest request)
    {
        var user = await _userRepo.GetByDeviceIdAsync(request.DeviceId);

        if (user == null)
        {
            var suffix = request.DeviceId.Length >= 8 ? request.DeviceId[..8] : request.DeviceId;
            user = new User
            {
                Email = $"guest_{request.DeviceId}@guest.local",
                Username = $"Guest_{suffix}",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                DeviceId = request.DeviceId
            };

            await _userRepo.CreateAsync(user);

            var player = new Player { UserId = user.Id };
            await _playerRepo.CreateAsync(player);
        }

        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            PlayerId = user.Id.ToString(),
            DisplayName = user.Username
        };
    }
}
