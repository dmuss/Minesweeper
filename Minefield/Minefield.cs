#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class Minefield
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int RemainingCellsToWin { get => _totalCells - _numMines - _revealedCells; }
#pragma warning disable CS8618 // Reset and re-initialised every time the GameScene is entered.
    public Cell[,] Cells { get; private set; }
#pragma warning restore CS8618

    private int _totalCells = 0;
    private int _revealedCells = 0;
    private int _numMines = 0;
    private bool _isFirstClick = true;

    private readonly Point[] _cellNeighbours =
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

    public void Reset(Difficulty difficulty)
    {
        InitCellGrid(difficulty);

        _totalCells = Width * Height;
        _revealedCells = 0;

        SetMines(difficulty);

        _isFirstClick = true;
    }

    public CellState? RevealCellAtPosition(Point gridLocation)
    {
        if (GetCellAtPosition(gridLocation) is Cell cell)
        {
            if (_isFirstClick)
            {
                _isFirstClick = false;

                if (cell.IsMine)
                {
                    RemoveMineAtPoint(gridLocation);
                    MoveMineToFirstAvailablePosition();
                }
            }

            RevealCell(cell);

            if (cell.IsEmpty) { FloodReveal(cell); }

            return cell.State;
        }
        else
        {
            return null;
        }
    }

    public void FlagCellAtPosition(Point gridLocation)
    {
        if (GetCellAtPosition(gridLocation) is Cell cell) { cell.ChangeFlagState(); }
    }

    public void Win()
    {
        foreach (Cell cell in Cells) { RevealCell(cell); }
    }

    public void Lose(Point mineToFlagLocation) { RevealMines(mineToFlagLocation); }

    private void InitCellGrid(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                {
                    Width = 9;
                    Height = 9;
                    break;
                }
            case Difficulty.Medium:
                {
                    Width = 16;
                    Height = 16;
                    break;
                }
            case Difficulty.Hard:
                {
                    Width = 30;
                    Height = 16;
                    break;
                }
            default:
                break;
        }

        Cells = new Cell[Height, Width];
        for (byte x = 0; x < Width; x++)
        {
            for (byte y = 0; y < Height; y++)
            {
                Cells[y, x] = new Cell(x, y);
            }
        }
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
            int xMax = Width;
            int yMax = Height;
            Point mineLocation = new(rand.Next(xMax), rand.Next(yMax));
            if (mines.Add(mineLocation)) { SetMineAtPoint(mineLocation); }
        }
    }

    private bool IsPointInGridBounds(Point point)
    {
        return (point.X >= 0 && point.X < Width) &&
               (point.Y >= 0 && point.Y < Height);
    }

    private Cell? GetCellAtPosition(Point gridPos)
    {
        return IsPointInGridBounds(gridPos) ? Cells[gridPos.Y, gridPos.X] : null;
    }

    private void SetMineAtPoint(Point mineLocation)
    {
        if (GetCellAtPosition(mineLocation) is Cell cell)
        {
            cell.SetAsMine();

            foreach (Point dir in _cellNeighbours)
            {
                Point neighbourCoords = new(mineLocation.X + dir.X, mineLocation.Y + dir.Y);

                if (GetCellAtPosition(neighbourCoords) is Cell neighbour)
                {
                    neighbour.AddAdjacentMine();
                }
            }
        }
    }

    private void RemoveMineAtPoint(Point mineLocation)
    {
        if (GetCellAtPosition(mineLocation) is Cell cell)
        {
            cell.SetAsEmpty();

            foreach (Point dir in _cellNeighbours)
            {
                Point neighbourCoords = new(mineLocation.X + dir.X, mineLocation.Y + dir.Y);
                if (GetCellAtPosition(neighbourCoords) is Cell neighbour)
                {
                    if (neighbour.IsMine)
                    {
                        cell.AddAdjacentMine();
                    }
                    else
                    {
                        neighbour.RemoveAdjacentMine();
                    }
                }
            }
        }
    }

    private void MoveMineToFirstAvailablePosition()
    {
        for (byte y = 0; y < Height; y++)
        {
            for (byte x = 0; x < Width; x++)
            {
                Point gridPosition = new Point(x, y);

                if (GetCellAtPosition(gridPosition) is Cell cell)
                {
                    if (!cell.IsMine)
                    {
                        SetMineAtPoint(gridPosition);
                        return;
                    }
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
        if (visited.Contains(cell) || !cell.IsEmpty) { return; }

        visited.Add(cell);

        if (cell.IsNotRevealed) { RevealCell(cell); }

        foreach (Point dir in _cellNeighbours)
        {
            Point neighbourLocation = new(cell.MinefieldPos.X + dir.X, cell.MinefieldPos.Y + dir.Y);
            if (GetCellAtPosition(neighbourLocation) is Cell neighbour)
            {
                RecurseFloodReveal(neighbour, visited);
            }
        }
    }

    private void RevealNeighboursOfVisitedCells(HashSet<Cell> visited)
    {
        foreach (Cell visitedCell in visited)
        {
            foreach (Point dir in _cellNeighbours)
            {
                Point neighbourLocation = new(visitedCell.MinefieldPos.X + dir.X, visitedCell.MinefieldPos.Y + dir.Y);
                if (GetCellAtPosition(neighbourLocation) is Cell neighbour)
                {
                    if (!visited.Contains(neighbour) && neighbour.IsNotRevealed)
                    {
                        RevealCell(neighbour);
                    }
                }
            }
        }
    }

    private void RevealCell(Cell cell)
    {
        cell.Reveal();
        _revealedCells++;
    }

    private void RevealMines(Point mineToFlagLocation)
    {
        // For minefields large enough it would probably be more efficient to store and iterate through only mine locations,
        // to avoid unnecessary iterations and possible branching performance hit. But currently the game has a maximum of
        // 480 cells, so this should be fine.
        foreach (Cell cell in Cells) { if (cell.IsMine) { RevealCell(cell); } }

        if (GetCellAtPosition(mineToFlagLocation) is Cell mineToFlag) { mineToFlag.SetAsRevealedMine(); }
    }
}