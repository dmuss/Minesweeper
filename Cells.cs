using System;
using System.Collections.Generic;
using System.Linq;
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
        public int X { get; init; }
        public int Y { get; init; }
        public bool IsRevealed { get; set; }
        public CellFlag Flagged { get; set; }

        private int _value;

        public Cell(int x, int y)
        {
            Value = 0;
            X = x;
            Y = y;
            IsRevealed = false;
            Flagged = CellFlag.NotFlagged;
        }
    }

    /// <summary>
    /// Stores and provides coordinate points to get neighbouring cells in the grid.
    /// </summary>
    private static class Directions
    {
        public static Point[] AllDirections { get => _directions; }
        public static Point[] CardinalDirections { get => _directions.Take(4).ToArray(); }

        private static readonly Point[] _directions =
        {
            new(-1, 0),  // W
            new(0, -1),  // N
            new(1, 0),   // E
            new(0, 1),   // S
            new(-1, -1), // NW
            new(1, -1),  // NE
            new(1, 1),   // SE
            new(-1, 1),  // SW
        };

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

        // TODO: if revealed cell is a mine, game over

        if (cell.Value == 0) { FloodReveal(args.GridLocation.X, args.GridLocation.Y); }
    }

    private void InitCells(int width, int height)
    {
        _cells = new Cell[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _cells[y, x] = new(x, y);
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

            foreach (var dir in Directions.AllDirections)
            {
                var neighbourX = mine.X + dir.X;
                var neighbourY = mine.Y + dir.Y;

                bool neighbourInXBounds = neighbourX >= 0 && neighbourX < Width;
                bool neighbourinYBounds = neighbourY >= 0 && neighbourY < Height;

                if (neighbourInXBounds && neighbourinYBounds)
                {
                    _cells[neighbourY, neighbourX].Value++;
                }
            }
        }
    }

    private void FloodReveal(int x, int y)
    {
        HashSet<Cell> visited = new();

        RecurseFloodReveal(x, y, visited);

        foreach (var cell in visited)
        {
            foreach (var dir in Directions.AllDirections)
            {
                var neighbourX = cell.X + dir.X;
                var neighbourY = cell.Y + dir.Y;

                bool neighbourInXBounds = neighbourX >= 0 && neighbourX < Width;
                bool neighbourinYBounds = neighbourY >= 0 && neighbourY < Height;

                if (neighbourInXBounds && neighbourinYBounds)
                {
                    _cells[neighbourY, neighbourX].IsRevealed = true;
                }
            }
        }

    }

    private void RecurseFloodReveal(int x, int y, HashSet<Cell> visited)
    {
        bool inXBounds = x >= 0 && x < Width;
        bool inYBounds = y >= 0 && y < Height;
        if (!inXBounds || !inYBounds || visited.Contains(_cells[y, x])) { return; }
        if (_cells[y, x].Value != 0) { return; }

        _cells[y, x].IsRevealed = true;
        visited.Add(_cells[y, x]);

        foreach (var dir in Directions.AllDirections)
        {
            RecurseFloodReveal(x + dir.X, y + dir.Y, visited);
        }
    }
}