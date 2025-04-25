using Privaseek.Domain.Enums;

namespace Privaseek.Domain.ValueObjects;

public record ResultItem(
    string Title,
    string Subtitle,
    string Icon,
    double Score,
    string Path,
    DateTime Timestamp,
    ResultType Type);
