namespace Privaseek.Domain.Entities;

public class IndexedFile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
