namespace Privaseek.Applications.Contracts;

public interface IAppUsageIndexer
{
    /// <summary>Indexe l’historique d’usage des apps (Android only).</summary>
    Task IndexAsync(CancellationToken ct = default);
}
