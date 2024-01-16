using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;

namespace PicoLife.Data;

public partial class SeedDatabase
{
    public async Task<List<SeedItem>> GetItemsAsync()
    {
        await Init();
        return await Database.Table<SeedItem>().ToListAsync();
    }

    public async Task<SeedItem> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<SeedItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(SeedItem item)
    {
        await Init();
        if (item.ID != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(SeedItem item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}