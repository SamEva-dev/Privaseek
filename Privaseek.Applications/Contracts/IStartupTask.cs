﻿namespace Privaseek.Applications.Contracts;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
