using Microsoft.Xna.Framework;

namespace Minesweeper;

public partial class Minefield
{
    private class Cell
    {
        public static byte MineValue { get; } = 9;
        public static byte NoAdjacentMineValue { get; } = 0;

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
            Rect = new(Constants.CellSize * x, Constants.CellSize * y, Constants.CellSize, Constants.CellSize);
            X = x;
            Y = y;
        }
    }
}