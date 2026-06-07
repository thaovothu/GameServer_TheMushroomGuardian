using System.Security.Claims;
using GameServer.DTOs.Player;
using GameServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.Controllers;

[ApiController]
[Route("api/player")]
[Authorize]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService) => _playerService = playerService;

    [HttpGet("data")]
    public async Task<IActionResult> GetData()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        try
        {
            var data = await _playerService.GetPlayerDataAsync(userId.Value);
            return Ok(data);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("data")]
    public async Task<IActionResult> SaveData([FromBody] PlayerDataDto request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        try
        {
            await _playerService.SavePlayerDataAsync(userId.Value, request);
            return Ok(new { message = "Player data saved" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return claim != null ? int.Parse(claim) : null;
    }
}
