using System;

namespace Minesweeper;

public class SceneChangeArgs : EventArgs
{
    public SceneChangeArgs(SceneManager.Scenes scene) => Scene = scene;
    public SceneManager.Scenes Scene { get; init; }
}