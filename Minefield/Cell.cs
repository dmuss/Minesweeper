using System;
using Microsoft.Xna.Framework;

namespace Minesweeper;

/// <summary>
/// Represents the state of the cell. The unerlying integral values of this enum
/// are used to access the array of sprite rectangles stored in the spritesheet.
/// </summary>
public enum CellState : byte
{
    Empty,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Mine,
    RevealedMine,
    Unrevealed,
    Flagged,
    Question
}

/// <summary>
/// Represents an individual cell in a minefield. Holds and controls a cell's state and provides
/// information required to render the cell.
/// </summary>
public class Cell
{
    #region Rendering
    /// <summary>
    /// The size in pixels to render individual cells on the minefield. Used to determine the size of the
    /// rendered `Rectangle` of a given cell, as well as adjust window size for different difficulties.
    /// </summary>
    public static int SizeInPixels { get; } = 50;

    /// <summary>
    /// The `Rectangle` of this cell to be used to render itself as part of the minefield.
    /// </summary>
    public Rectangle Rect { get; init; }
    #endregion Rendering

    public bool IsEmpty { get => _value == (int)CellState.Empty; }
    public bool IsMine { get => _value == (int)CellState.Mine; }
    public bool IsNotRevealed { get => State == CellState.Unrevealed; }

    public CellState State { get; private set; } = CellState.Unrevealed;
    public Point MinefieldPos { get; init; }

    /// <summary>
    /// The underlying integral value of the cell, regardless of whether it's unrevealed, flagged, or questioned.
    /// When revealed, this value is used to determine which value of the `CellState` enum should be assigned to
    /// the cell, which is used to determine the correct source rectangle in the spritesheet.
    /// </summary>
    private byte _value;

    public Cell(byte x, byte y)
    {
        _value = (int)CellState.Empty;
        Rect = new(SizeInPixels * x, SizeInPixels * y, SizeInPixels, SizeInPixels);
        MinefieldPos = new Point(x, y);
    }

    /// <summary>
    /// When a mine is found adjacent to this cell, increase it's underlying value by one, clamping
    /// the result from [0-9].
    /// </summary>
    public void AddAdjacentMine() => _value = (byte)MathHelper.Clamp(_value + 1, 0, (byte)CellState.Mine);

    /// <summary>
    /// When a mine is removed adjacent to this cell, decrease it's underlying value by one, clamping
    /// the result from [0-9].
    /// </summary>
    public void RemoveAdjacentMine() => _value = (byte)MathHelper.Clamp(_value - 1, 0, (byte)CellState.Mine);

    public void SetAsEmpty() => _value = (int)CellState.Empty;

    public void SetAsMine() => _value = (int)CellState.Mine;

    /// <summary>
    /// Modifies this cell's state so that it renders a revealed cell with the correct
    /// underlying value.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Underlying values are clamped, but in the case that the revealed cell's underlying
    /// integral value is not defined in the `CellState` enum, throw an exception and crash out.
    /// </exception>
    public void Reveal()
    {
        try
        {
            if (Enum.IsDefined(typeof(CellState), _value))
            {
                State = (CellState)_value;
            }
        }
        catch (SystemException e)
        {
            throw new ArgumentException("Revealed cell has an invalid underlying value.", e);
        }
    }

    public void SetAsRevealedMine() => State = CellState.RevealedMine;

    /// <summary>
    /// Called when a player right click's a cell. Cycles through the valid unrevealed
    /// cell states.
    /// </summary>
    public void ChangeFlagState()
    {
        switch (State)
        {
            case CellState.Unrevealed:
                State = CellState.Flagged;
                break;
            case CellState.Flagged:
                State = CellState.Question;
                break;
            case CellState.Question:
                State = CellState.Unrevealed;
                break;
            default:
                break;
        }
    }
}