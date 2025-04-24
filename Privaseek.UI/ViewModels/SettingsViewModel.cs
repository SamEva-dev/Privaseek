using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Privaseek.Applications.Contracts;
using Privaseek.Infrastructure.Providers;

namespace Privaseek.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IEnumerable<IStartupTask> _startupTasks;

    public SettingsViewModel(IEnumerable<IStartupTask> startupTasks)
    {
        _startupTasks = startupTasks;
    }

    [ObservableProperty]
    private bool _isReindexing;

    [RelayCommand]
    private async Task ReindexAsync()
    {
        if (IsReindexing) return;
        IsReindexing = true;

        foreach (var task in _startupTasks)
        {
            // On ne relance que la tâche d’indexation ; 
            // si plusieurs tâches d’indexation, on les exécute toutes
            if (task is FileIndexStartupTask)
                await task.ExecuteAsync();
        }

        IsReindexing = false;
    }
}
