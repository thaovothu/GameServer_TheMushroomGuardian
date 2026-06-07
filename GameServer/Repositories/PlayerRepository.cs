using GameServer.Data;
using GameServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;

    public PlayerRepository(AppDbContext context) => _context = context;

    public async Task<Player?> GetByUserIdAsync(int userId) =>
        await _context.Players.FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<Player> CreateAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task UpdateAsync(Player player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
    }
}
