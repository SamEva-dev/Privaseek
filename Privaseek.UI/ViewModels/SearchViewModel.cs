using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Privaseek.Applications.Contracts;
using Privaseek.Domain.Enums;
using Privaseek.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Privaseek.UI.ViewModels;

public class Grouping<K, T> : ObservableCollection<T>
{
    public K Key { get; }
    public Grouping(K key, IEnumerable<T> items) : base(items) => Key = key;
}

public partial class SearchViewModel : ObservableObject
{
    private readonly ISearchService _searchService;

    public SearchViewModel(ISearchService searchService)
    {
        _searchService = searchService;
    }
    [ObservableProperty] 
    private bool _showFiles = true;

    [ObservableProperty] 
    private bool _showMessages = true;

    [ObservableProperty] 
    private bool _showApps = true;

    [ObservableProperty]
    private string _query = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] 
    private DateTime? _startDate;

    [ObservableProperty] 
    private DateTime? _endDate;

    // Option groupement
    [ObservableProperty] 
    private bool _groupByDate;

    public ObservableCollection<ResultItem> Results { get; } = new();

    // Résultats groupés
    public ObservableCollection<Grouping<string, ResultItem>> GroupedResults { get; } = new();

    [RelayCommand]
    private async Task SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        IsLoading = true;
        Results.Clear();
        GroupedResults.Clear();

        try
        {
            var items = await _searchService.QueryAsync(query, CancellationToken.None);

            // 1) application du filtre date
            if (StartDate.HasValue)
                items = items.Where(r => r.Timestamp.Date >= StartDate.Value.Date).ToList();
            if (EndDate.HasValue)
                items = items.Where(r => r.Timestamp.Date <= EndDate.Value.Date).ToList();

            // 2) application du filtre type
            items = items.Where(r =>
                   (r.Type == ResultType.File && ShowFiles)
                || (r.Type == ResultType.Message && ShowMessages)
                || (r.Type == ResultType.AppUsage && ShowApps))
                .ToList();

            if (GroupByDate)
            {
                // groupement par section : Today / Yesterday / Older
                var grouped = items
                    .OrderByDescending(r => r.Timestamp)
                    .GroupBy(r => SectionOf(r.Timestamp))
                    .Select(g => new Grouping<string, ResultItem>(g.Key, g))
                    .ToList();

                foreach (var grp in grouped)
                    GroupedResults.Add(grp);
            }
            else
            {
                // affichage plat
                foreach (var it in items.OrderByDescending(r => r.Score))
                    Results.Add(it);
            }
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
        StartDate = null;
        EndDate = null;
        ShowFiles = ShowMessages = ShowApps = true;
        GroupByDate = false;
        Results.Clear();
        GroupedResults.Clear();
    }

    private static string SectionOf(DateTime dt)
    {
        var today = DateTime.Today;
        if (dt.Date == today) return "Aujourd’hui";
        if (dt.Date == today.AddDays(-1)) return "Hier";
        return "Plus ancien";
    }
}