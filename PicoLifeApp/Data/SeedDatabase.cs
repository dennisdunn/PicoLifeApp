using SQLite;
using PicoLife.Models;

namespace PicoLife.Data;

public class SeedDatabase
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
        var result = await Database.CreateTableAsync<SeedItem>();
    }

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