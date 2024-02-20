using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

internal class Cells
{
    private class Cell
    {
        public enum CellFlag { NotFlagged, Flag, Question };

        public int Value
        {
            get => _value;
            set
            {
                _value = MathHelper.Clamp(value, 0, 9);
            }
        }
        public bool IsRevealed { get; set; }
        public CellFlag Flagged { get; set; }

        private int _value;

        public Cell()
        {
            Value = 0;
            IsRevealed = true;
            Flagged = CellFlag.NotFlagged;
        }
    }

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
    public int ValueAt(int x, int y) { return _cells[y, x].Value; }

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

        HashSet<Point> mines = new(Constants.NumMines);
        while (mines.Count < 10)
        {
            var rand = new Random();
            Point point = new(rand.Next(Constants.NumMines - 1), rand.Next(Constants.NumMines - 1));
            if (!mines.Add(point)) { continue; }
        }

        foreach (var mine in mines)
        {
            _cells[mine.Y, mine.X].Value = 9;

            foreach (var point in Constants.AdjacentPoints)
            {
                var neighbourX = mine.X + point.X;
                var neighbourY = mine.Y + point.Y;

                bool neighbourInXBounds = neighbourX >= 0 && neighbourX < Width;
                bool neighbourinYBounds = neighbourY >= 0 && neighbourY < Height;

                if (neighbourInXBounds && neighbourinYBounds)
                {
                    _cells[neighbourY, neighbourX].Value++;
                }
            }
        }
    }

}