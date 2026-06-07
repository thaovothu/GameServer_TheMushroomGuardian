namespace GameServer.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
