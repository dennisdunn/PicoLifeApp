using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicoLife.Models;
using PicoLife.Services;
using PicoLife.Services.Database;
using Cell = PicoLife.Models.Cell;

namespace PicoLife.ViewModels;

[QueryProperty("Item", "Item")]
public partial class EditPageViewModel(DatabaseManager database, AlertService alert) : ObservableObject
{
    private readonly DatabaseManager database = database;
    private readonly AlertService alert = alert;
    [ObservableProperty]
    private Seed? item;

    [RelayCommand]
    private async Task DeleteSeed()
    {
        await database.DeleteSeedAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task SaveSeed()
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
            await alert.OK("Name can't be empty.", "Enter a value for the Name field.");
        else
            await database.SaveSeedAsync(Item);
    }

    [RelayCommand]
    private void AddCell()
    {
        Item.Cells.Add(new Cell());
    }

    [RelayCommand]
    private void RemoveCell(Cell cell)
    {
        if (cell.Id == 0)
        {
            // TODO add to deleted cells collection
        }
        Item.Cells.Remove(cell);
    }
}
