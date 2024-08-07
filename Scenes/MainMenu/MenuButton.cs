using System;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class MenuButton
{
    public bool IsDown { get; set; } = false;

    public Rectangle Rect { get; init; }
    public Color Colour { get; init; }
    public Action OnPress { get; init; }
    public string Label { get; init; }

    public MenuButton(string label, Rectangle rect, Color colour, Action onPress)
    {
        Label = label;
        Rect = rect;
        Colour = colour;
        OnPress = onPress;
    }

    public bool IsMouseInButton(Point mousePosition)
    {
        return (mousePosition.X >= Rect.X && mousePosition.X <= Rect.Right) &&
               (mousePosition.Y >= Rect.Y && mousePosition.Y <= Rect.Bottom);
    }
}