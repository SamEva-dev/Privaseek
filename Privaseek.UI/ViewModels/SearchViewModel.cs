using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Privaseek.Applications.Contracts;
using Privaseek.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Privaseek.UI.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    private readonly ISearchService _searchService;

    public SearchViewModel(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [ObservableProperty]
    private string _query = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    public ObservableCollection<ResultItem> Results { get; } = new();

    [RelayCommand]
    private async Task SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        IsLoading = true;
        Results.Clear();

        try
        {
            var items = await _searchService.QueryAsync(query, CancellationToken.None);
            foreach (var it in items)
                Results.Add(it);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Clear()
    {
        Query = string.Empty;
        Results.Clear();
    }
}