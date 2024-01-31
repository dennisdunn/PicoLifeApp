using PicoLife.Models;
using Cell = PicoLife.Models.Cell;
using SQLite;
using Java.Nio.Channels;
using System.Collections.ObjectModel;

namespace PicoLife.Services.Database;

public static class Constants
{
    public const string DatabaseFilename = "PicoLife.db3";

    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}

public partial class DatabaseManager
{
    private SQLiteAsyncConnection _dbConn;

    public async Task Init()
    {
        if (_dbConn is not null) return;

        _dbConn = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _dbConn.CreateTablesAsync<Seed, Cell>();
    }

    public async Task<IEnumerable<Seed>> GetSeedsAsync()
    {
        await Init();
        return await _dbConn.Table<Seed>().ToListAsync();
    }

    public async Task<Seed> GetSeedById(int seedId)
    {
        await Init();
        var seed = await _dbConn.Table<Seed>().FirstOrDefaultAsync(i => i.Id == seedId);
        var cells = await _dbConn.Table<Cell>().Where(c => c.SeedId == seedId).ToListAsync();

        seed.Cells = new ObservableCollection<Cell>(cells);
        return seed;
    }

    public async Task<int> Upsert(object item)
    {
        var prop = item.GetType().GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)));
        if (prop != null)
        {
            var value = prop.GetValue(item);
            if (value.Equals(0))
            {
                return await _dbConn.InsertAsync(item);
            }
            else
            {
                return await _dbConn.UpdateAsync(item);
            }
        }
        return 0;
    }

    public async Task SaveSeedAsync(Seed item)
    {
        await Init();

        await Upsert(item);
        foreach (var cell in item.Cells)
        {
            cell.SeedId = item.Id;
            await Upsert(cell);
        }
    }

    public async Task<int> DeleteSeedAsync(Seed item)
    {
        await Init();
        foreach (var cell in item.Cells) await _dbConn.DeleteAsync(cell);
        return await _dbConn.DeleteAsync(item);
    }
}