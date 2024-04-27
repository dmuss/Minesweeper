using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public abstract class BaseScene
{
    public bool IsCurrentScene { get; protected set; }
    public MSGame MSGame { get; protected set; }

    public BaseScene(in MSGame game) => MSGame = game;

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