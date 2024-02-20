using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene : IScene
{
    public event EventHandler<SceneChangeArgs> ChangeScene;
    public event EventHandler<MouseEventArgs> MouseEvent;

    public bool IsCurrentScene { get; protected set; }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);

    public virtual void Enter() { IsCurrentScene = true; }
    public virtual void Leave() { IsCurrentScene = false; }

    protected virtual void OnChangeScene(SceneChangeArgs e) { ChangeScene?.Invoke(this, e); }
    protected virtual void OnMouseEvent(object sender, MouseEventArgs e) { MouseEvent?.Invoke(this, e); }
}