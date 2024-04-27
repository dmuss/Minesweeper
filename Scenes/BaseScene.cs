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
        int requestedBufferWidth = Cell.Size * MSGame.Minefield.GridWidth;
        int requestedBufferHeight = Cell.Size * MSGame.Minefield.GridHeight;
        MSGame.SetBackBufferSize(requestedBufferWidth, requestedBufferHeight);

        IsCurrentScene = true;
        MSGame.MouseInput.Reset();
    }

    public virtual void Leave()
    {
        IsCurrentScene = false;
        MSGame.MouseInput.Reset();
    }
}