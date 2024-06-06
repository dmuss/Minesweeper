#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Minesweeper;

/// <summary>
/// Represents the playable minefield area.
/// </summary>
public class Minefield
{
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public int RemainingCellsToWin { get => _totalCells - _numMines - _revealedCells; }
#pragma warning disable CS8618 // Reset and re-initialised every time the GameScene is entered.
    public Cell[,] Cells { get; private set; }
#pragma warning restore CS8618

    private int _totalCells = 0;
    private int _revealedCells = 0;
    private int _numMines = 0;
    private bool _isFirstClick = true;

    /// <summary>
    /// An array of coordinates whose values can be used to find all the neighbouring cells of a
    /// given cell.
    /// </summary>
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

    /// <summary>
    /// Resets the minefield by reinitialising the cell grid with a new minefield
    /// and resetting all underlying state.
    /// </summary>
    /// <param name="difficulty">
    /// The difficulty determines the size of the cell grid and number of mines
    /// found in the minefield.
    /// </param>
    public void Reset(Difficulty difficulty)
    {
        InitCellGrid(difficulty);

        _totalCells = GridWidth * GridHeight;
        _revealedCells = 0;
        _isFirstClick = true;

        SetMines(difficulty);
    }

    /// <summary>
    /// Reveal the cell at a given grid position in the minefield, if that position
    /// is a valid cell, including first click protection.
    /// </summary>
    /// <param name="gridPosition">
    /// The location of the cell to be revealed.
    /// </param>
    /// <returns>
    /// Returns the nullable <c>CellState</c> of the cell at <paramref name="gridPosition">, if any.
    /// </returns>
    public CellState? RevealCellAtPosition(Point gridPosition)
    {
        if (GetCellAtPosition(gridPosition) is Cell cell)
        {
            // If a mine is revealed on the first click, it will be moved and the minefield
            // will be updated to ensure the player cannot lose on the first click.
            if (_isFirstClick)
            {
                _isFirstClick = false;

                if (cell.IsMine)
                {
                    RemoveMineAtPoint(gridPosition);
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

    /// <summary>
    /// Changes the state of a valid unrevealed cell so the player can flag that cell as suspected
    /// mine or a questioned mine.
    /// </summary>
    /// <param name="gridPosition">
    /// The position of the cell in the minefield grid to be flagged.
    /// </param>
    public void FlagCellAtPosition(Point gridPosition)
    {
        if (GetCellAtPosition(gridPosition) is Cell cell) { cell.ChangeFlagState(); }
    }

    /// <summary>
    /// When the player has won (i.e. all non-mine cells have been revealed), reveal
    /// the remainder of the minefield.
    /// </summary>
    public void Win()
    {
        foreach (Cell cell in Cells) { RevealCell(cell); }
    }

    /// <summary>
    /// When the player loses (i.e. reveals a cell that is a mine), flag that cell as
    /// a revealed mine and reveal the remaining mines on the minefield.
    /// </summary>
    /// <param name="mineToFlagLocation">
    /// Location of the cell to be flagged as the revealed mine.
    /// </param>
    public void Lose(Point mineToFlagLocation) { RevealMines(mineToFlagLocation); }

    /// <summary>
    /// Based on the requested difficulty, reinitialise the minefield's cell grid
    /// with new, empty cells.
    /// </summary>
    /// <param name="difficulty">
    /// The requested game difficulty.
    /// </param>
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

        Cells = new Cell[GridHeight, GridWidth];
        for (byte x = 0; x < GridWidth; x++)
        {
            for (byte y = 0; y < GridHeight; y++)
            {
                Cells[y, x] = new Cell(x, y);
            }
        }
    }

    /// <summary>
    /// Based on the requested difficulty, randomly place the correct amount of mines in
    /// the minefield and adjust the adjacent cells' underlying values.
    /// </summary>
    /// <param name="difficulty">
    /// The requested game difficulty.
    /// </param>
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

    private bool IsPointInGridBounds(Point point)
    {
        return (point.X >= 0 && point.X < GridWidth) &&
               (point.Y >= 0 && point.Y < GridHeight);
    }

    /// <summary>
    /// Validates that a requested cell at the provided position exists, and returns it.
    /// </summary>
    /// <param name="gridPosition">
    /// The position of the requested cell.
    /// </param>
    /// <returns>
    /// Returns a reference to the requested cell if it is valid, otherwise returns <c>null</c>.
    /// </returns>
    private Cell? GetCellAtPosition(Point gridPosition)
    {
        return IsPointInGridBounds(gridPosition) ? Cells[gridPosition.Y, gridPosition.X] : null;
    }

    /// <summary>
    /// Sets the cell at a requested location as a mine if that position is valid given the
    /// current size of the minefield and adjusts the value of its neighbouring cells.
    /// </summary>
    /// <param name="mineLocation">
    /// The location in the minefield to set as a mine.
    /// </param>
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

    /// <summary>
    /// Sets the cell at a requested location as empty if that position is valid given the
    /// current size of the minefield and adjusts the value of its neighbouring cells.
    /// </summary>
    /// <param name="mineLocation">
    /// The location in the minefield to set as a mine.
    /// </param>
    private void RemoveMineAtPoint(Point mineLocation)
    {
        if (GetCellAtPosition(mineLocation) is Cell cell)
        {
            cell.SetAsEmpty();

            foreach (Point dir in _cellNeighbours)
            {
                Point neighbourCoords = new(mineLocation.X + dir.X, mineLocation.Y + dir.Y);

                // If the new cell's neighbour is a mine, its underlying integral value needs
                // to be increased, otherwise decrease each neighbour's underlying integral
                // value. 
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

    /// <summary>
    /// Tries to find a valid position for a mine, starting in the top-left corner of the minefield.
    /// If that space is occupied by a mine, move right across the current row. If that row is filled
    /// with mines, repeat this process moving down the rows.
    /// </summary>
    private void MoveMineToFirstAvailablePosition()
    {
        for (byte y = 0; y < GridHeight; y++)
        {
            for (byte x = 0; x < GridWidth; x++)
            {
                Point gridPosition = new(x, y);

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

    /// <summary>
    /// If a revealed cell is empty, reveal all neighbouring empty cells and those cells' neighbouring
    /// cells.
    /// </summary>
    /// <param name="cell">
    /// The originating cell to flood reveal from.
    /// </param>
    private void FloodReveal(Cell cell)
    {
        HashSet<Cell> visited = new();
        RecurseFloodReveal(cell, visited);
        RevealNeighboursOfVisitedCells(visited);
    }

    /// <summary>
    /// Recursively reveal every empty cell adjacent to a provided cell.
    /// </summary>
    /// <param name="cell">
    /// The initial empty cell.
    /// </param>
    /// <param name="visited">
    /// The set of empty cells already visited.
    /// </param>
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

    /// <summary>
    /// After all empty cells are flood revealed, reveal the neighbouring cells at the edges if they are not
    /// already revealed.
    /// </summary>
    /// <param name="visited">
    /// The set of all empty cells revealed in the initial flood reveal.
    /// </param>
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

    /// <summary>
    /// Reveals all the mines in a minefield and flags the requested location as the mine that was
    /// revealed by the player.
    /// </summary>
    /// <param name="mineToFlagPosition">
    /// The position of the mined cell that the player revealed.
    /// </param>
    private void RevealMines(Point mineToFlagPosition)
    {
        // For minefields large enough it would probably be more efficient to store and iterate through only mine locations,
        // to avoid unnecessary iterations and possible branching performance hit. But currently the game has a maximum of
        // 480 cells, so this should be fine.
        foreach (Cell cell in Cells) { if (cell.IsMine) { RevealCell(cell); } }

        if (GetCellAtPosition(mineToFlagPosition) is Cell mineToFlag) { mineToFlag.SetAsRevealedMine(); }
    }
}