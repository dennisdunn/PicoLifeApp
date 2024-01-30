using Cell = PicoLife.Models.Cell;

namespace PicoLife.Services;

public partial class SeedDatabase
{
    public async Task<List<Cell>> GetCellsBySeedIdAsync(int seedId)
    {
        await Init();
        return await Database.Table<Cell>().Where(i => i.SeedId == seedId).ToListAsync();
    }

    public async Task<int> SaveCellAsync(Cell item)
    {
        await Init();
        return await Upsert(item);
    }

    public async Task<int> DeleteCellAsync(Cell item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }

    public async Task<int> DeleteCellAsync(IEnumerable<Cell> items)
    {
        await Init();
        foreach (var item in items)
        {
            await Database.DeleteAsync(item);
        }
        return 0;
    }
}