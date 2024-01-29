using SQLite;
using SQLiteNetExtensions;
using PicoLife.Models;

namespace PicoLife.Services;

public partial class SeedDatabase
{
    public async Task<List<Models.Cell>> GetItemsAsync()
    {
        await Init();
        return await Database.Table<Models.Cell>().ToListAsync();
    }

    public async Task<Models.Cell> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<Models.Cell>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(Models.Cell item)
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

    public async Task<int> DeleteItemAsync(Models.Cell item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}