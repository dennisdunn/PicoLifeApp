using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;

namespace PicoLife.Services;

public partial class SeedDatabase
{
    public async Task<List<Seed>> GetCollectionsAsync()
    {
        await Init();
        return await Database.Table<Seed>().ToListAsync();
    }

    public async Task<Seed> GetCollectionAsync(int id)
    {
        await Init();
        return await Database.Table<Seed>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveCollectionAsync(Seed Collection)
    {
        await Init();
        if (Collection.ID != 0)
        {
            return await Database.UpdateAsync(Collection);
        }
        else
        {
            return await Database.InsertAsync(Collection);
        }
    }

    public async Task<int> DeleteCollectionAsync(Seed Collection)
    {
        await Init();

        foreach (var item in Collection.Cells)
        {
            await DeleteItemAsync(item);
        }

        return await Database.DeleteAsync(Collection);
    }
}