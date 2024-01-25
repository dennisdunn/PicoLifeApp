using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;

namespace PicoLife.Data;

public partial class SeedDatabase
{
    public async Task<List<SeedCollection>> GetCollectionsAsync()
    {
        await Init();
        return await Database.Table<SeedCollection>().ToListAsync();
    }

    public async Task<SeedCollection> GetCollectionAsync(int id)
    {
        await Init();
        return await Database.Table<SeedCollection>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveCollectionAsync(SeedCollection Collection)
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

    public async Task<int> DeleteCollectionAsync(SeedCollection Collection)
    {
        await Init();

        foreach (var item in Collection.Seeds)
        {
            await DeleteItemAsync(item);
        }

        return await Database.DeleteAsync(Collection);
    }
}