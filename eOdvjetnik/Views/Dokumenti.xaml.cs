using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;

namespace eOdvjetnik.Views;

public partial class Dokumenti : ContentPage
{
    DocsDatabase database;
    public ObservableCollection<DocsItem> Items { get; set; } = new();
    public Dokumenti(DocsDatabase docsDatabase)
    {
        InitializeComponent();
        database = docsDatabase;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        });
    }
    async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = new DocsItem()
        });
    }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not DocsItem item)
            return;

        await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>
        {
            ["item"] = item
        });
    }
}