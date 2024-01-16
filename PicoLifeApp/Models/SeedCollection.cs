using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace PicoLife.Models;

public class SeedCollection

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }

    [OneToMany]
    public List<SeedItem> Seeds { get; set; }
}
