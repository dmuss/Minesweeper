using Microsoft.Xna.Framework;

namespace Minesweeper;

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
    Hidden,
    Flagged,
    Question
}

public class Cell
{
    public static int Size { get; } = 50;
    public static int HalfSize { get; } = Size / 2;

    public bool IsEmpty { get => _value == (int)CellState.Empty; }
    public bool IsMine { get => _value == (int)CellState.Mine; }
    public bool IsHidden { get => State == CellState.Hidden; }

    public CellState State { get; private set; } = CellState.Hidden;
    public int X { get; init; }
    public int Y { get; init; }
    public Rectangle Rect { get; init; }

    // Underlying value of cell representing whether cell is a mine or has adjacent mines.
    private int _value;

    #region Public Methods
    public Cell(byte x, byte y)
    {
        _value = (int)CellState.Empty;
        Rect = new(Size * x, Size * y, Size, Size);
        X = x;
        Y = y;
    }

    public void AddAdjacentMine() => _value = MathHelper.Clamp(++_value, 0, (int)CellState.Mine);

    public void SetAsMine() => _value = (int)CellState.Mine;

    public void Reveal() => State = (CellState)_value;

    public void FlagAsRevealedMine() => State = CellState.RevealedMine;

    public void ChangeFlagState()
    {
        switch (State)
        {
            case CellState.Hidden:
                {
                    State = CellState.Flagged;
                    break;
                }
            case CellState.Flagged:
                {
                    State = CellState.Question;
                    break;
                }
            case CellState.Question:
                {
                    State = CellState.Hidden;
                    break;
                }
            default:
                break;
        }
    }
    #endregion Public Methods
}