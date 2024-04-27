using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene : IScene
{
    public event EventHandler<SceneChangeArgs>? ChangeScene;

    public bool IsCurrentScene { get; protected set; }
    public MouseInputManager MouseInput { get; protected set; }

    public BaseScene(in MSGame game)
    {
        MouseInput = game.MouseInput;
    }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);

    public virtual void Enter()
    {
        IsCurrentScene = true;
        MouseInput.Reset();
    }

    public virtual void Leave()
    {
        IsCurrentScene = false;
        MouseInput.Reset();
    }

    protected virtual void OnChangeScene(SceneChangeArgs e) { ChangeScene?.Invoke(this, e); }
}