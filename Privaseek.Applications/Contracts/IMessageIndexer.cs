namespace Privaseek.Applications.Contracts;

public interface IMessageIndexer
{
    /// <summary>Indexe tous les SMS/MMS accessibles sur l’appareil.</summary>
    Task IndexAsync(CancellationToken ct = default);
}
