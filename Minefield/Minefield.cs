using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class Minefield
{
    public byte GridWidth { get; private set; }
    public byte GridHeight { get; private set; }
    public bool PlayerHasWon { get => _playerHasWon; }

    private Cell[,] _cells;
    private ushort _totalCells = 0;
    private ushort _revealedCells = 0;
    private byte _numMines = 0;
    private bool _playerHasWon = false;
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

    public int? RevealCellAtPosition(Point mousePosition)
    {
        Point cellLocation = new(
            (int)MathF.Floor(mousePosition.X / Cell.Size),
            (int)MathF.Floor(mousePosition.Y / Cell.Size));

        if (GetCellAtPoint(cellLocation) is Cell cell)
        {
            cell.State = CellState.Revealed;
            _revealedCells++;

            if (cell.Value == Cell.NoAdjacentMineValue)
            {
                FloodReveal(cell);
            }

            if (_revealedCells == _totalCells - _numMines) { _playerHasWon = true; }

            return cell.Value;
        }
        else
        {
            return null;
        }
    }

    public void FlagCellAtPosition(Point mousePosition)
    {
        Point cellLocation = new(
            (int)MathF.Floor(mousePosition.X / Cell.Size),
            (int)MathF.Floor(mousePosition.Y / Cell.Size));

        if (GetCellAtPoint(cellLocation) is Cell cell) { cell.ChangeFlagState(); }
    }

    public void RevealMines(Point mousePosition)
    {
        Point cellLocation = new(
            (int)MathF.Floor(mousePosition.X / Cell.Size),
            (int)MathF.Floor(mousePosition.Y / Cell.Size));

        if (GetCellAtPoint(cellLocation) is Cell cell)
        {
            if (!PlayerHasWon) { cell.SetAsRevealedMine(); }

            // Reveal all other mines.
            for (byte x = 0; x < GridWidth; x++)
            {
                for (byte y = 0; y < GridHeight; y++)
                {
                    Point cellSearch = new(x, y);
                    if (GetCellAtPoint(cellSearch) is Cell validCell && validCell.Value == Cell.MineValue)
                    {
                        validCell.State = CellState.Revealed;
                    }
                }
            }
        }
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
        for (byte x = 0; x < GridWidth; x++)
        {
            for (byte y = 0; y < GridHeight; y++)
            {
                _cells[y, x] = new Cell(x, y);
            }
        }

        _totalCells = (ushort)(GridWidth * GridHeight);
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

    private void FloodReveal(Cell cell)
    {
        HashSet<Cell> visited = new();
        RecurseFloodReveal(cell, visited);
        RevealNeighboursOfVisitedCells(visited);
    }

    private void RecurseFloodReveal(Cell cell, HashSet<Cell> visited)
    {
        if (visited.Contains(cell) || cell.Value != Cell.NoAdjacentMineValue) { return; }

        if (cell.State == CellState.Hidden)
        {
            cell.State = CellState.Revealed;
            _revealedCells++;
        }
        visited.Add(cell);

        foreach (Point dir in _neighbours)
        {
            Point neighbourLocation = new(cell.X + dir.X, cell.Y + dir.Y);
            if (GetCellAtPoint(neighbourLocation) is Cell neighbour)
            {
                RecurseFloodReveal(neighbour, visited);
            }
        }
    }

    private void RevealNeighboursOfVisitedCells(HashSet<Cell> visited)
    {
        foreach (Cell visitedCell in visited)
        {
            foreach (Point dir in _neighbours)
            {
                Point neighbourLocation = new(visitedCell.X + dir.X, visitedCell.Y + dir.Y);
                if (GetCellAtPoint(neighbourLocation) is Cell neighbour)
                {
                    if (!visited.Contains(neighbour) && neighbour.State == CellState.Hidden)
                    {
                        neighbour.State = CellState.Revealed;
                        _revealedCells++;
                    }
                }
            }
        }
    }
}