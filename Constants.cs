using Microsoft.Xna.Framework;

internal static class Constants
{
    public const ushort InitialWindowWidth = 900;
    public const ushort InitialWindowHeight = 900;

    public const ushort RenderWidth = InitialWindowWidth;
    public const ushort RenderHeight = InitialWindowHeight;
    public const float AspectRatio = (float)RenderWidth / RenderHeight;

    public const byte CellSize = RenderWidth / 9;

    public const byte NumMines = 10;
    public static readonly Color[] CellColours =
    {
        Color.Gray,
        Color.Blue,
        Color.Green,
        Color.Red,
        Color.Navy,
        Color.Maroon,
        Color.Teal,
        Color.Purple,
        Color.Black,
        Color.Yellow
    };
}