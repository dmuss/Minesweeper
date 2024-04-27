using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

// TODO: Find a place for this.
public enum Difficulty { Easy, Medium, Hard }

public class Minefield
{
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public Color[] CellColours { get => _cellColours; }

    private Cell[,] _cells;
    private int _totalCells = 0;
    private int _revealedCells = 0;
    private byte _numMines = 0;
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

#pragma warning disable CS8618 // _cells initialised in separate function to allow for easy difficulty changes.
    public Minefield(Difficulty difficulty = Difficulty.Easy)
    {
        Reset(difficulty);
    }
#pragma warning restore CS8618

    public void Reset(Difficulty difficulty = Difficulty.Easy)
    {
        InitCellGrid(difficulty);
        SetMines(difficulty);
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
            (int)MathF.Floor(mousePosition.X / Cell.Size),
            (int)MathF.Floor(mousePosition.Y / Cell.Size));

        if (GetCellAtPoint(cellLocation) is Cell cell)
        {
            cell.IsRevealed = true;

            if (cell.Value == Cell.NoAdjacentMineValue)
            {
                FloodReveal(cellLocation);
            }
            else
            {
                _revealedCells++;
            }

            return cell.Value;
        }
        else
        {
            return null;
        }
    }

    public bool PlayerHasWon()
    {
        // TODO: Check for flagged squares.
        if (_revealedCells == _totalCells - _numMines)
        {
            return true;
        }

        return false;
    }

    private void InitCellGrid(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                {
                    GridWidth = 9;
                    GridHeight = 9;
                    break;
                }
            case Difficulty.Medium:
                {
                    GridWidth = 16;
                    GridHeight = 16;
                    break;
                }
            case Difficulty.Hard:
                {
                    GridWidth = 30;
                    GridHeight = 16;
                    break;
                }
            default:
                break;
        }

        _cells = new Cell[GridHeight, GridWidth];
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                _cells[y, x] = new Cell(x, y);
            }
        }

        _totalCells = GridWidth * GridHeight;
        _revealedCells = 0;
    }

    private void SetMines(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                {
                    _numMines = 10;
                    break;
                }
            case Difficulty.Medium:
                {
                    _numMines = 40;
                    break;
                }
            case Difficulty.Hard:
                {
                    _numMines = 99;
                    break;
                }
            default:
                break;
        }

        HashSet<Point> mines = new(_numMines);
        Random rand = new();

        while (mines.Count < _numMines)
        {
            int xMax = GridWidth;
            int yMax = GridHeight;
            Point mineLocation = new(rand.Next(xMax), rand.Next(yMax));
            if (mines.Add(mineLocation)) { SetMineAtPoint(mineLocation); }
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
            _revealedCells++;

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
                    if (!visited.Contains(neighbour) && !neighbour.IsRevealed)
                    {
                        neighbour.IsRevealed = true;
                        _revealedCells++;
                    }
                }
            }
        }
    }
}