using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;
using System.Collections.ObjectModel;

namespace PicoLife.Services;

public partial class SeedDatabase
{
    public async Task<List<Seed>> GetSeedsAsync()
    {
        await Init();
        return await Database.Table<Seed>().ToListAsync();
    }

    public async Task<Seed> GetSeedByIdAsync(int id)
    {
        await Init();

        var seed = await Database.Table<Seed>().Where(i => i.ID == id).FirstOrDefaultAsync();
        var cells = await GetCellsBySeedIdAsync(seed.ID);
        seed.Cells = new ObservableCollection<Models.Cell>(cells);
        return seed;
    }

    public async Task<int> SaveSeedAsync(Seed seed)
    {
        await Init();
        await Upsert(seed);

        foreach (var cell in seed.Cells)
        {
            cell.SeedId=seed.ID;
            await SaveCellAsync(cell);
        }

        return 0;
    }

    public async Task<int> DeleteSeedAsync(Seed seed)
    {
        await Init();

        foreach (var item in seed.Cells)
        {
            await DeleteCellAsync(item);
        }

        return await Database.DeleteAsync(seed);
    }
}