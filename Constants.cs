internal static class Constants
{
    public const ushort InitialWindowWidth = 900;
    public const ushort InitialWindowHeight = 900;

    public const ushort RenderWidth = InitialWindowWidth;
    public const ushort RenderHeight = InitialWindowHeight;
    public const float AspectRatio = (float)RenderWidth / RenderHeight;

    public const byte CellSize = RenderWidth / 9;

    public const byte NumMines = 10;
}