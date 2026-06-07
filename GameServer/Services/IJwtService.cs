using GameServer.Models;

namespace GameServer.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
