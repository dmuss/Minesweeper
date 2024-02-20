using System;

namespace Minesweeper;

public class SceneChangeArgs : EventArgs
{
    public SceneChangeArgs(Scenes scene) => Scene = scene;
    public Scenes Scene { get; init; }
}