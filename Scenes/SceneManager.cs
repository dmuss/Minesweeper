using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class SceneManager
{
    public enum Scenes { MainMenu, Game };

    private readonly Dictionary<Scenes, BaseScene> _scenes;

    private BaseScene _currentScene;

    public SceneManager(MSGame game)
    {
        _scenes = new Dictionary<Scenes, BaseScene>{
            { Scenes.MainMenu, new MainMenuScene(game) },
            { Scenes.Game, new GameScene(game) },
        };
        Debug.Assert(_scenes.Count == Enum.GetNames(typeof(Scenes)).Length,
                     "Scene manager does not have an instance of all required game scenes. Make sure to update the Scenes enum.");

        _currentScene = _scenes[Scenes.MainMenu];
        _currentScene.ChangeScene += HandleSceneChangeEvents;
        _currentScene.Enter();
    }

    public void Update(GameTime gameTime) { _currentScene.Update(gameTime); }

    public void Draw(SpriteBatch spriteBatch) { _currentScene.Draw(spriteBatch); }

    private void HandleSceneChangeEvents(object sender, SceneChangeArgs e) { SwitchScene(e.Scene); }

    private void SwitchScene(Scenes scene)
    {
        _currentScene.Leave();
        _currentScene = _scenes[scene];
        _currentScene.ChangeScene += HandleSceneChangeEvents;
        _currentScene.Enter();
    }

}