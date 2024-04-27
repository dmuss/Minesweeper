internal static class Constants
{
    public const ushort InitialWindowWidth = 900;
    public const ushort InitialWindowHeight = 900;

    //public const ushort RenderWidth = InitialWindowWidth;
    //public const ushort RenderHeight = InitialWindowHeight;
    //public const float AspectRatio = (float)RenderWidth / RenderHeight;

    // TODO: Difficulties:
    // 8x8
    // 9x9
    // 16x16
    // 30x16
    // TODO: CellSize needs to be split by width and height.
    public const byte CellSize = InitialWindowWidth / 9;
    public const byte NumMines = 10;
}