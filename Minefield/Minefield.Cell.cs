using Microsoft.Xna.Framework;

namespace Minesweeper;

public partial class Minefield
{
    private class Cell
    {
        # region Nested
        public enum CellFlag { NotFlagged, Flag, Question };
        #endregion

        public static int MineValue = 9;
        public static int NoAdjacentMine = 0;

        # region Properties
        public int Value
        {
            get => _value;
            set { _value = MathHelper.Clamp(value, 0, MineValue); }
        }
        public int X { get; init; }
        public int Y { get; init; }
        public bool IsRevealed { get; set; }
        public CellFlag Flagged { get; set; }
        #endregion

        #region Fields
        private int _value;
        #endregion

        # region Constructors
        public Cell(int x, int y)
        {
            Value = 0;
            X = x;
            Y = y;
            IsRevealed = false;
            Flagged = CellFlag.NotFlagged;
        }
        #endregion
    }
}