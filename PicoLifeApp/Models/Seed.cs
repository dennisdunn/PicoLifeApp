using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace PicoLife.Models;

public class Seed

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }

    [OneToMany]
    public ObservableCollection<Cell> Cells { get; set; } = [];

    public override string ToString()
    {
        return $"[{string.Join(",",Cells.Select(s=>s.ToString()))}]";
    }
}
