namespace GameServer.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
    public string Role { get; set; } = "player"; // "player" | "admin"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Player? Player { get; set; }
}
