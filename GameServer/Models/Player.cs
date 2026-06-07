namespace GameServer.Models;

public class Player
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestId { get; set; } = 1;
    public int StepId { get; set; } = 1;
    public int Coins { get; set; } = 0;
    public string InventoryJson { get; set; } = "{}";
    public string SkillsCsv { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
