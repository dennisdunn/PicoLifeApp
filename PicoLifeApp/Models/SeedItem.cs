using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PicoLife.Models;

public class SeedItem

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    [ForeignKey(typeof(SeedCollection))]
    public int CollectionId { get; set; }
}
