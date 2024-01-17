using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace PicoLife.Models;

public class SeedCollection

{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }

    [OneToMany]
    public ObservableCollection<SeedItem> Seeds { get; set;}=new ObservableCollection<SeedItem>();
}
