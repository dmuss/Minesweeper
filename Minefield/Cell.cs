using Microsoft.Xna.Framework;

namespace Minesweeper;

public class Cell
{
    public static int MineValue { get; } = 9;
    public static int NoAdjacentMineValue { get; } = 0;
    public static int Size { get; } = 50;
    public static int HalfSize { get; } = Size / 2;

    public bool IsRevealed { get; set; } = false;

    public int X { get; init; }
    public int Y { get; init; }
    public Rectangle Rect { get; init; }

    public int Value
    {
        get => _value;
        set { _value = MathHelper.Clamp(value, 0, MineValue); }
    }

    private int _value;

    public Cell(int x, int y)
    {
        Value = NoAdjacentMineValue;
        Rect = new(Size * x, Size * y, Size, Size);
        X = x;
        Y = y;
    }
}