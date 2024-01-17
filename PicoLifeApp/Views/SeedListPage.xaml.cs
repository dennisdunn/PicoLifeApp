using System.Collections.ObjectModel;
using PicoLife.Data;
using PicoLife.Models;

namespace PicoLife.Views;

public partial class SeedListPage : ContentPage
{
    SeedDatabase database;
    public ObservableCollection<SeedCollection> Items { get; set; } = new();

    public SeedListPage(SeedDatabase SeedCollectionDatabase)
	{
        InitializeComponent();
        database = SeedCollectionDatabase;
        BindingContext = this;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetCollectionsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

        });
    }
    async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SeedEditPage), true, new Dictionary<string, object>
        {
            ["Item"] = new SeedCollection()
        });
    }

    private async void  CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not SeedCollection item)
            return;

        var seeds = await database.GetItemsAsync();
        seeds = seeds.Where(seed => seed.CollectionId == item.ID).ToList();
        foreach (var seed in seeds)
        {
            item.Seeds.Add(seed);
        }

        await Shell.Current.GoToAsync(nameof(SeedEditPage), true, new Dictionary<string, object>
        {
            ["Item"] = item
        });
    }
}

