using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;

namespace PicoLife.Models;

public partial class Seed : ObservableObject

{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? notes;

    [Ignore]
    public ObservableCollection<Cell> Cells { get; set; } = [];

    public override string ToString()
    {
        return $"[{string.Join(",", Cells.Select(s => s.ToString()))}]";
    }
}
