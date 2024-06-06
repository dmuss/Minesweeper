using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public enum Scenes { MainMenu, Game };

/// <summary>
/// Controls and updates the current game scene.
/// </summary>
public static class SceneManager
{
    private static Dictionary<Scenes, BaseScene> _scenes = new();
    private static BaseScene _currentScene;

    static SceneManager()
    {
        _scenes = new()
        {
            {Scenes.MainMenu, new MainMenuScene()},
            {Scenes.Game, new GameScene()},
        };

        _currentScene = _scenes[Scenes.MainMenu];
        _currentScene.Enter();
    }

    public static void Update(GameTime gameTime)
    {
        _currentScene.Update(gameTime);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        _currentScene.Draw(spriteBatch);
    }

    public static void SwitchScene(Scenes scene)
    {
        _currentScene.Leave();
        _currentScene = _scenes[scene];
        _currentScene.Enter();
    }
}