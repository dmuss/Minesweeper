internal class Cell
{
    public enum CellFlag { NotFlagged, Flag, Question };

    public byte Value { get; private set; } // 0-9, 9 is mine
    public bool IsRevealed { get; private set; }
    public CellFlag Flagged { get; private set; }
}