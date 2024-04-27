using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class Minefield
{
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public Color[] CellColours { get => _cellColours; }

    private Cell[,] _cells;
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

    private readonly Color[] _cellColours =
    {
        Color.Gray,
        Color.Blue,
        Color.Green,
        Color.Red,
        Color.Navy,
        Color.Maroon,
        Color.Teal,
        Color.Purple,
        Color.Chartreuse,
        Color.Yellow
    };

    public Minefield(int minefieldWidth, int minefieldHeight)
    {
        GridWidth = minefieldWidth;
        GridHeight = minefieldHeight;

        // Initialise cell grid.
        _cells = new Cell[GridHeight, GridWidth];
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                _cells[y, x] = new Cell(x, y);
            }
        }

        // Place mines.
        HashSet<Point> mines = new(Constants.NumMines);
        Random rand = new();

        while (mines.Count < Constants.NumMines)
        {
            int xMax = GridWidth;
            int yMax = GridHeight;
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

    public int? RevealCell(Point mousePosition)
    {
        Point cellLocation = new(
            (int)MathF.Floor(mousePosition.X / Constants.CellSize),
            (int)MathF.Floor(mousePosition.Y / Constants.CellSize));

        if (GetCellAtPoint(cellLocation) is Cell cell)
        {
            cell.IsRevealed = true;

            if (cell.Value == Cell.NoAdjacentMineValue) { FloodReveal(cellLocation); }

            return cell.Value;
        }
        else
        {
            return null;
        }
    }

    private bool PointInGridBounds(Point point)
    {
        return (point.X >= 0 && point.X < GridWidth) &&
               (point.Y >= 0 && point.Y < GridHeight);
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
                Point neighbour = new(cellLocation.X + dir.X, cellLocation.Y + dir.Y);
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
}