using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public partial class Minefield
{
    private class Grid
    {
        public int Width { get; init; }
        public int Height { get; init; }

        private readonly Cell[,] _cells;
        private readonly Point[] _directions =
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

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new Cell[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _cells[y, x] = new(x, y);
                }
            }

            CreateMinefield();
        }

        public bool IsRevealed(int x, int y) { return _cells[y, x].IsRevealed; }
        public int ValueAt(int x, int y) { return _cells[y, x].Value; }

        public void RevealCell(int x, int y)
        {
            if (CoordsInBounds(x, y))
            {
                _cells[y, x].IsRevealed = true;

                // TODO: If revealed cell is a mine, game over.

                if (_cells[y, x].Value == Cell.NoAdjacentMine) { FloodReveal(x, y); }
            }
        }

        private void CreateMinefield()
        {
            HashSet<Point> mines = new(Constants.NumMines);
            while (mines.Count < Constants.NumMines)
            {
                Random rand = new();
                int coordMax = Width; // TODO: Separate X and Y for different play sizes.
                Point mineLocation = new(rand.Next(coordMax), rand.Next(coordMax));
                if (mines.Add(mineLocation)) { SetMine(mineLocation.X, mineLocation.Y); }
            }
        }

        private void SetMine(int x, int y)
        {
            // TODO: Remove this magic number.
            _cells[y, x].Value = Cell.MineValue;

            foreach (var dir in _directions)
            {
                int neighbourX = x + dir.X;
                int neighbourY = y + dir.Y;
                if (CoordsInBounds(neighbourX, neighbourY)) { _cells[neighbourY, neighbourX].Value++; }
            }
        }

        private void FloodReveal(int x, int y)
        {
            HashSet<Cell> visited = new();
            RecurseFloodReveal(x, y, visited);
            RevealVisitedNeighbours(visited);
        }

        private void RecurseFloodReveal(int x, int y, HashSet<Cell> visited)
        {
            if (!CoordsInBounds(x, y)) { return; }

            var cell = _cells[y, x];

            if (visited.Contains(cell) || cell.Value != Cell.NoAdjacentMine) { return; }

            cell.IsRevealed = true;
            visited.Add(cell);

            foreach (var dir in _directions) { RecurseFloodReveal(x + dir.X, y + dir.Y, visited); }
        }

        private void RevealVisitedNeighbours(HashSet<Cell> visited)
        {
            foreach (var cell in visited)
            {
                foreach (var dir in _directions)
                {
                    var neighbourX = cell.X + dir.X;
                    var neighbourY = cell.Y + dir.Y;
                    if (CoordsInBounds(neighbourX, neighbourY))
                    {
                        _cells[neighbourY, neighbourX].IsRevealed = true;
                    }
                }
            }
        }

        private bool CoordsInBounds(int x, int y) { return (x >= 0 && x < Width) && (y >= 0 && y < Height); }
    }
}