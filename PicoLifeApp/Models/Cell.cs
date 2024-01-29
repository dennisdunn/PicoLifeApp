using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicoLife.Models;

public class Cell

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Color { get; set; }

    [ForeignKey(typeof(Seed))]
    public int CollectionId { get; set; }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}
