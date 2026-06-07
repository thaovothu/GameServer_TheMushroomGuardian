using GameServer.DTOs.Player;
using GameServer.Models;
using GameServer.Repositories;

namespace GameServer.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepo;

    public PlayerService(IPlayerRepository playerRepo) => _playerRepo = playerRepo;

    public async Task<PlayerDataDto> GetPlayerDataAsync(int userId)
    {
        var player = await _playerRepo.GetByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Player not found");

        return new PlayerDataDto
        {
            QuestId = player.QuestId,
            StepId = player.StepId,
            Coins = player.Coins,
            InventoryJson = player.InventoryJson,
            SkillsCsv = player.SkillsCsv
        };
    }

    public async Task SavePlayerDataAsync(int userId, PlayerDataDto data)
    {
        var player = await _playerRepo.GetByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Player not found");

        player.QuestId = data.QuestId;
        player.StepId = data.StepId;
        player.Coins = data.Coins;
        player.InventoryJson = data.InventoryJson;
        player.SkillsCsv = data.SkillsCsv;

        await _playerRepo.UpdateAsync(player);
    }
}
