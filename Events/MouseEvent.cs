using System;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class MouseEventArgs : EventArgs
{
    //public MouseEventArgs(Point gridLocation) => GridLocation = gridLocation;

    public Point GridLocation { get; init; }
    public bool LeftButtonDown { get; init; }
    public bool LeftButtonClick { get; init; }
    public bool RightButtonDown { get; init; }
    public bool RightButtonClick { get; init; }
}