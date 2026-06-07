using GameServer.DTOs.Player;

namespace GameServer.Services;

public interface IPlayerService
{
    Task<PlayerDataDto> GetPlayerDataAsync(int userId);
    Task SavePlayerDataAsync(int userId, PlayerDataDto data);
}
