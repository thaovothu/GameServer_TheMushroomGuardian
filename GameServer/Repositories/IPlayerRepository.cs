using GameServer.Models;

namespace GameServer.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByUserIdAsync(int userId);
    Task<Player> CreateAsync(Player player);
    Task UpdateAsync(Player player);
}
