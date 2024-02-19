using System;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public class MouseEventArgs : EventArgs
{
    public MouseEventArgs(Point gridLocation) => GridLocation = gridLocation;

    public Point GridLocation { get; init; }
}