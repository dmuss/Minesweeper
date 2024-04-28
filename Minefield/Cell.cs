using Microsoft.Xna.Framework;

namespace Minesweeper;

public enum CellState { Hidden, Flagged, Question, Revealed }

public class Cell
{
    public static byte NoAdjacentMineValue { get; } = 0;
    public static byte MineValue { get; } = 9;
    public static byte RevealedMineValue { get; } = 10;
    public static byte HiddenValue { get; } = 11;
    public static byte FlaggedValue { get; } = 12;
    public static byte QuestionValue { get; } = 13;

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
}