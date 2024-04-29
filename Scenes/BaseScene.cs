using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene
{
    protected bool IsCurrentScene { get; private set; }
    protected MSGame MSGame { get; /*protected set;*/ }

    protected BaseScene(in MSGame game) => MSGame = game;

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);

    public virtual void Enter()
    {
        IsCurrentScene = true;
        MSGame.MouseInput.Reset();
    }

    public virtual void Leave()
    {
        IsCurrentScene = false;
        MSGame.MouseInput.Reset();
    }
}