namespace Privaseek.Domain.Entities;

public class IndexedMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Sender { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime SentDate { get; set; } = DateTime.UtcNow;
}
