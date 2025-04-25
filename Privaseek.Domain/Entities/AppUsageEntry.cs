namespace Privaseek.Domain.Entities;

public class AppUsageEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PackageName { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public DateTime LastTimeUsed { get; set; } = DateTime.UtcNow;
}
