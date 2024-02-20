using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

internal class Cells
{
    private class Cell
{
    public enum CellFlag { NotFlagged, Flag, Question };

        public byte Value { get; set; } // 0-8, >=9 is mine
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

    // TODO: Is there a more idiomatic way to do this then an empty init body?
    public int Width { get => _cells.GetLength(1); init { } }
    public int Height { get => _cells.GetLength(0); init { } }

    public Cells(Game1 game, int width, int height)
    {
        Width = width;
        Height = height;
        InitCells(width, height);
        game.RaiseMouseEvent += HandleMouseEvent;
    }

    public bool IsRevealed(int x, int y) { return _cells[y, x].IsRevealed; }

    private void HandleMouseEvent(object sender, MouseEventArgs args)
    {
        var cell = _cells[args.GridLocation.Y, args.GridLocation.X];
        cell.IsRevealed = true;
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