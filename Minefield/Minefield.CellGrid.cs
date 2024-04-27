using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public partial class Minefield
{
    private class CellGrid
    {
        public int Width { get; init; }
        public int Height { get; init; }

        private readonly Cell[,] _cells;
        private readonly Point[] _neighbours =
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

        public CellGrid(int width, int height)
        {
            Width = width;
            Height = height;

            // Initialise cell grid.
            _cells = new Cell[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _cells[y, x] = new Cell(x, y);
                }
            }

            // Place mines.
            HashSet<Point> mines = new(Constants.NumMines);
            Random rand = new();

            while (mines.Count < Constants.NumMines)
            {
                int xMax = Width;
                int yMax = Height;
                Point mineLocation = new(rand.Next(xMax), rand.Next(yMax));
                if (mines.Add(mineLocation)) { SetMineAtPoint(mineLocation); }
            }
        }

        public Cell? GetCellAtPoint(Point cellLocation)
        {
            if (!PointInGridBounds(cellLocation))
            {
                return null;
            }
            else
            {
                return _cells[cellLocation.Y, cellLocation.X];
            }
        }

        public void RevealCell(Point cellLocation)
        {
            if (GetCellAtPoint(cellLocation) is Cell cell)
            {
                // TODO: If revealed cell is a mine, game over.
                cell.IsRevealed = true;

                if (cell.Value == Cell.NoAdjacentMineValue) { FloodReveal(cellLocation); }
            }
        }

        private void FloodReveal(Point cellLocation)
        {
            HashSet<Cell> visited = new();
            RecurseFloodReveal(cellLocation, visited);

            RevealNeighboursOfVisitedCells(visited);
        }

        private void RecurseFloodReveal(Point cellLocation, HashSet<Cell> visited)
        {
            if (GetCellAtPoint(cellLocation) is Cell cell)
            {
                if (visited.Contains(cell) || cell.Value != Cell.NoAdjacentMineValue) { return; }

                cell.IsRevealed = true;
                visited.Add(cell);

                foreach (Point dir in _neighbours)
                {
                    Point neighbour = new Point(cellLocation.X + dir.X, cellLocation.Y + dir.Y);
                    RecurseFloodReveal(neighbour, visited);
                }
            }
            else
            {
                return; // Out of bounds, return.
            }

        }

        private void RevealNeighboursOfVisitedCells(HashSet<Cell> visited)
        {
            foreach (Cell visitedCell in visited)
            {
                foreach (Point dir in _neighbours)
                {
                    if (GetCellAtPoint(new Point(visitedCell.X + dir.X, visitedCell.Y + dir.Y)) is Cell neighbour)
                    {
                        neighbour.IsRevealed = true;
                    }
                }
            }
        }

        private void SetMineAtPoint(Point mineLocation)
        {
            if (GetCellAtPoint(mineLocation) is Cell cell)
            {
                cell.Value = Cell.MineValue;

                foreach (Point dir in _neighbours)
                {
                    Point neighbourCoords = new(mineLocation.X + dir.X, mineLocation.Y + dir.Y);

                    if (GetCellAtPoint(neighbourCoords) is Cell neighbour)
                    {
                        neighbour.Value++;
                    }
                }
            }
        }

        private bool PointInGridBounds(Point point)
        {
            return (point.X >= 0 && point.X < Width) &&
                   (point.Y >= 0 && point.Y < Height);
        }
    }
}