using System;

namespace Minesweeper;

public class Cell
{
    public enum CellFlag { NotFlagged, Flag, Question };

    public byte Value { get; set; } // 0-9, 9 is mine
    public bool IsRevealed { get; set; }
    public CellFlag Flagged { get; set; }

    public Cell()
    {
        Value = 0;
        IsRevealed = false;
        Flagged = CellFlag.NotFlagged;
    }
}


internal class Cells
{
    private Cell[,] _cells;

    // TODO: Nullable get?
    public int Width { get => _cells.GetLength(1); init { } }
    public int Height { get => _cells.GetLength(0); init { } }
    public Cell[,] CellGrid { get => _cells; init { } }

    private void HandleMouseEvent(object sender, MouseEventArgs args)
    {
        Console.WriteLine($"Received event with grid location: ({args.GridLocation.X}, {args.GridLocation.Y})");
        var cell = _cells[args.GridLocation.Y, args.GridLocation.X];
        cell.IsRevealed = true;

        Console.WriteLine($"\tCell at grid is revealed: {cell.IsRevealed} || Has value: {cell.Value} || Flagged: {cell.Flagged}");
    }

    public Cells(Game1 game, int width, int height)
    {
        Width = width;
        Height = height;
        InitCells(width, height);
        game.RaiseMouseEvent += HandleMouseEvent;
    }

    private void InitCells(int width, int height)
    {
        _cells = new Cell[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _cells[y, x] = new();
            }
        }
    }

}