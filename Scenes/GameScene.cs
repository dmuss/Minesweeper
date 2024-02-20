using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    // TODO: Move mouse events to base class.
    protected event EventHandler<MouseEventArgs> SceneRaiseMouseEvent;

    private Minefield _mineField;

    public GameScene(in MSGame game)
    {
        game.RaiseMouseEvent += ScenePassMouseEvent;
        _mineField = new(game, 9, 9, ref SceneRaiseMouseEvent);
    }

    public override void Update(GameTime gameTime) { return; }

    public override void Draw(SpriteBatch spriteBatch) { _mineField.Draw(spriteBatch); }

    private void ScenePassMouseEvent(object sender, MouseEventArgs args)
    {
        if (IsCurrentScene)
        {
            EventHandler<MouseEventArgs> raiseMouseEvent = SceneRaiseMouseEvent;
            if (raiseMouseEvent != null) { raiseMouseEvent(this, args); }
        }
    }
}