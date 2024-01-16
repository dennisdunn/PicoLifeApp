using System.Collections.ObjectModel;
using PicoLife.Data;
using PicoLife.Models;

namespace PicoLife.Views;

public partial class SeedListPage : ContentPage
{
    SeedDatabase database;
    public ObservableCollection<SeedItem> Items { get; set; } = new();
    public SeedListPage(SeedDatabase SeedItemDatabase)
	{
        InitializeComponent();
        database = SeedItemDatabase;
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
        await Shell.Current.GoToAsync(nameof(SeedItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = new SeedItem()
        });
    }

    private async void  CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not SeedItem item)
            return;

        await Shell.Current.GoToAsync(nameof(SeedItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = item
        });
    }
}

