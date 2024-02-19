using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _pixel;
    private SpriteFont _font;

    private Cells _cells;

    public event EventHandler<MouseEventArgs> RaiseMouseEvent;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Constants.InitialWindowWidth;
        _graphics.PreferredBackBufferHeight = Constants.InitialWindowHeight;
        _graphics.ApplyChanges();

        _cells = new(this, 9, 9);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new(GraphicsDevice, 1, 1);
        _pixel.SetData(new Color[] { Color.White });

        _font = Content.Load<SpriteFont>("SilkscreenFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Mouse clicks are not restricted to the game window, so this crashes the game w/ an out-of-index
        // access if you click outside the window.
        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            var gridX = (int)MathF.Floor(mouseState.X / Constants.CellSize);
            var gridY = (int)MathF.Floor(mouseState.Y / Constants.CellSize);
            MouseEventArgs eventArgs = new(new Point(gridX, gridY));
            OnRaiseMouseEvent(eventArgs);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        // Draw cells.
        for (var y = 0; y < _cells.Height; y++)
        {
            for (var x = 0; x < _cells.Width; x++)
            {
                string cellLocation = x + "," + y;
                Rectangle cellRect = new(Constants.CellSize * x, Constants.CellSize * y, Constants.CellSize, Constants.CellSize);
                Color cellColour = Color.White;
                if (!_cells.CellGrid[y, x].IsRevealed)
                {
                    cellColour = Color.Purple;
                }
                _spriteBatch.Draw(_pixel, cellRect, cellColour);
                _spriteBatch.DrawString(_font, cellLocation, new Vector2(cellRect.X, cellRect.Y), Color.Black);
            }
        }

        // Draw grid lines.
        for (var y = 0; y < _cells.Height; y++)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(0, y * Constants.CellSize, Constants.RenderWidth, 1), Color.Black * 0.25f);
        }
        for (var x = 0; x < _cells.Width; x++)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(x * Constants.CellSize, 0, 1, Constants.RenderHeight), Color.Black * 0.25f);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void OnRaiseMouseEvent(MouseEventArgs args)
    {
        // Make a temporary copy to avoid the possibility of a race condition if the last subscriber
        // unsubscribes immediately after the null check and before the event is raised.
        EventHandler<MouseEventArgs> raiseMouseEvent = RaiseMouseEvent;

        if (raiseMouseEvent != null) { raiseMouseEvent(this, args); }
    }
}
