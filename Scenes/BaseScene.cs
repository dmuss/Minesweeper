using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene : IScene
{
    protected event EventHandler<SceneChangeArgs> SceneChange;

    public bool IsCurrentScene { get; protected set; }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);

    public virtual void Enter() { IsCurrentScene = true; }
    public virtual void Leave() { IsCurrentScene = false; }
}