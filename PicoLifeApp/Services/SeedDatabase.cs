using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;

namespace PicoLife.Services;

public partial class SeedDatabase
{
    SQLiteAsyncConnection Database;

    public SeedDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
       await Database.CreateTablesAsync<Seed, Models.Cell>();
    }
}