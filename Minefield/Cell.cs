using Microsoft.Xna.Framework;

namespace Minesweeper;

public enum CellState : byte { Revealed, Hidden = 11, Flagged, Question }

public class Cell
{
    public static byte NoAdjacentMineValue { get; } = 0;
    public static byte MineValue { get; } = 9;
    public static byte RevealedMineValue { get; } = 10;

    public static byte Size { get; } = 50;
    public static byte HalfSize { get; } = (byte)(Size / 2);

    public CellState State { get; set; } = CellState.Hidden;

    public byte X { get; init; }
    public byte Y { get; init; }
    public Rectangle Rect { get; init; }

    public byte Value
    {
        get => _value;
        set { _value = (byte)MathHelper.Clamp(value, 0, MineValue); }
    }

    private byte _value;

    public Cell(byte x, byte y)
    {
        Value = NoAdjacentMineValue;
        Rect = new(Size * x, Size * y, Size, Size);
        X = x;
        Y = y;
    }

    public void SetAsRevealedMine()
    {
        _value = RevealedMineValue;
    }

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
            case CellState.Revealed:
            default:
                break;
        }
    }
}