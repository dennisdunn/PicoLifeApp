using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace PicoLife.Models;

public partial class Cell : ObservableObject

{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private int x;

    [ObservableProperty]
    private int y;

    [Indexed]
    public int SeedId { get; set; }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}
