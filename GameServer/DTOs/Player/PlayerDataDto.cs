namespace GameServer.DTOs.Player;

public class PlayerDataDto
{
    public int QuestId { get; set; }
    public int StepId { get; set; }
    public int Coins { get; set; }
    public string InventoryJson { get; set; } = "{}";
    public string SkillsCsv { get; set; } = string.Empty;
}
