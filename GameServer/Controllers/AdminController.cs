using System.Security.Claims;
using GameServer.DTOs.Player;
using GameServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IPlayerRepository _playerRepo;

    public AdminController(IUserRepository userRepo, IPlayerRepository playerRepo)
    {
        _userRepo = userRepo;
        _playerRepo = playerRepo;
    }

    // GET /api/admin/players — danh sách tất cả player
    [HttpGet("players")]
    public async Task<IActionResult> GetAllPlayers()
    {
        var users = await _userRepo.GetAllAsync();
        var result = users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            u.Role,
            u.DeviceId,
            IsGuest = u.DeviceId != null,
            u.CreatedAt,
            Player = u.Player == null ? null : new
            {
                u.Player.QuestId,
                u.Player.StepId,
                u.Player.Coins,
                u.Player.InventoryJson,
                u.Player.SkillsCsv,
                u.Player.UpdatedAt
            }
        });
        return Ok(result);
    }

    // DELETE /api/admin/players/{id} — xóa player (cascade xóa Player)
    [HttpDelete("players/{id:int}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id == me) return BadRequest(new { message = "Không thể tự xóa tài khoản admin." });

        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "Player không tồn tại." });

        await _userRepo.DeleteAsync(user);
        return Ok(new { message = $"Đã xóa player #{id} ({user.Username})." });
    }

    // PUT /api/admin/players/{id}/data — override data player
    [HttpPut("players/{id:int}/data")]
    public async Task<IActionResult> OverridePlayerData(int id, [FromBody] PlayerDataDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "Player không tồn tại." });

        var player = await _playerRepo.GetByUserIdAsync(id);
        if (player == null) return NotFound(new { message = "Player data không tồn tại." });

        player.QuestId      = dto.QuestId;
        player.StepId       = dto.StepId;
        player.Coins        = dto.Coins;
        player.InventoryJson = dto.InventoryJson ?? "{}";
        player.SkillsCsv    = dto.SkillsCsv ?? string.Empty;
        player.UpdatedAt    = DateTime.UtcNow;

        await _playerRepo.UpdateAsync(player);
        return Ok(new { message = $"Đã cập nhật data cho player #{id} ({user.Username})." });
    }

    // PUT /api/admin/players/{id}/role — thay đổi role
    [HttpPut("players/{id:int}/role")]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] ChangeRoleRequest request)
    {
        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id == me) return BadRequest(new { message = "Không thể tự thay đổi role của mình." });

        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "Player không tồn tại." });

        if (request.Role != "player" && request.Role != "admin")
            return BadRequest(new { message = "Role không hợp lệ. Chỉ chấp nhận: player, admin." });

        user.Role = request.Role;
        await _userRepo.UpdateAsync(user);
        return Ok(new { message = $"Đã đổi role của {user.Username} → {request.Role}." });
    }
}

public class ChangeRoleRequest
{
    public string Role { get; set; } = "player";
}
