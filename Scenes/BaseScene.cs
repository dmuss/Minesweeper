using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene
{
    protected bool IsCurrentScene { get; private set; }
    protected MSGame MSGame { get; init; }

    protected BaseScene(MSGame game)
    {
        MSGame = game;
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
}