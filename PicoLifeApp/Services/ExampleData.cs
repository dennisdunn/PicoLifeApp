using PicoLife.Models;
using Cell = PicoLife.Models.Cell;

namespace PicoLife.Services;

public class ExampleData
{
    public static readonly IEnumerable<Cell> Cells = [
        new Cell { SeedId = 1, ID = 1, X = 7, Y = 2 },
        new Cell { SeedId = 1, ID = 2, X = 7, Y = 2 },
        new Cell { SeedId = 1, ID = 3, X = 7, Y = 2 },
        new Cell { SeedId = 2, ID = 4, X = 7, Y = 2 },
        new Cell { SeedId = 2, ID = 5, X = 7, Y = 3 },
        new Cell { SeedId = 2, ID = 6, X = 8, Y = 2 },
        new Cell { SeedId = 2, ID = 7, X = 8, Y = 3 },
        new Cell { SeedId = 3, ID = 8, X = 1, Y = 0 },
        new Cell { SeedId = 3, ID = 9, X = 2, Y = 1 },
        new Cell { SeedId = 3, ID = 10, X = 0, Y = 2 },
        new Cell { SeedId = 3, ID = 11, X = 1, Y = 2 },
        new Cell { SeedId = 3, ID = 12, X = 2, Y = 2 },
        new Cell { SeedId = 4, ID = 13, X = 5, Y = 2 },
        new Cell { SeedId = 4, ID = 14, X = 6, Y = 2 },
        new Cell { SeedId = 4, ID = 15, X = 5, Y = 3 },
        new Cell { SeedId = 4, ID = 16, X = 8, Y = 4 },
        new Cell { SeedId = 4, ID = 17, X = 7, Y = 5 },
        new Cell { SeedId = 4, ID = 18, X = 8, Y = 5 },
        new Cell { SeedId = 5, ID = 19, X = 5, Y = 2 },
        new Cell { SeedId = 5, ID = 20, X = 6, Y = 2 },
        new Cell { SeedId = 5, ID = 21, X = 7, Y = 2 },
        new Cell { SeedId = 5, ID = 22, X = 7, Y = 3 },
        new Cell { SeedId = 5, ID = 23, X = 6, Y = 3 },
        new Cell { SeedId = 5, ID = 24, X = 8, Y = 3 },
        new Cell { SeedId = 6, ID = 19, X = 8, Y = 3 },
        new Cell { SeedId = 6, ID = 20, X = 7, Y = 3 },
        new Cell { SeedId = 6, ID = 21, X = 7, Y = 4 },
        new Cell { SeedId = 6, ID = 22, X = 6, Y = 4 },
        new Cell { SeedId = 6, ID = 23, X = 7, Y = 5 },
    ];


    public static readonly IEnumerable<Seed> Seeds = [
        new Seed
        {
            ID = 1,
            Name = "Blinker",
        },
        new Seed
        {
            ID = 2,
            Name = "Block",
        },
        new Seed
        {
            ID = 3,
            Name = "Glider",
        },
        new Seed
        {
            ID = 4,
            Name = "Beacon",
        },
        new Seed
        {
            ID = 5,
            Name = "Toad",
        },
        new Seed
        {
            ID = 6,
            Name = "R-Pentonimo",
        },
    ];
}
