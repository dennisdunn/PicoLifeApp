using SQLite;

namespace PicoLife.Models;

public class SeedItem

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public string Name { get; set; }
    public string Notes { get; set; }
}
