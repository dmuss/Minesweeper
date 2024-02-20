using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class SceneManager
{
    public enum Scenes { MainMenu, Game };

    private readonly Dictionary<Scenes, IScene> _scenes;

    private IScene _currentScene;

    public SceneManager(MSGame game)
    {
        _scenes = new Dictionary<Scenes, IScene>{
            { Scenes.MainMenu, new MainMenuScene(game) },
            { Scenes.Game, new GameScene(game) },
        };
        Debug.Assert(_scenes.Count == Enum.GetNames(typeof(Scenes)).Length,
                     "Scene manager does not have an instance of all required game scenes. Make sure to update the Scenes enum.");

        _currentScene = _scenes[Scenes.MainMenu];
        _currentScene.Enter();
    }

    public void SwitchScene(Scenes scene)
    {
        _currentScene.Leave();
        _currentScene = _scenes[scene];
        _currentScene.Enter();
    }

    public void Update(GameTime gameTime) { _currentScene.Update(gameTime); }

    public void Draw(SpriteBatch spriteBatch) { _currentScene.Draw(spriteBatch); }
}