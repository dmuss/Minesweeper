using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class MSGame : Game
{
    public event EventHandler<MouseEventArgs> RaiseMouseEvent;

    public SpriteFont Font { get => _font; }
    public Texture2D Pixel { get => _pixel; }

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _pixel;
    private SpriteFont _font;
    private Minefield _minefield;

    public MSGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // NOTE: base.Initialize() calls Game.LoadContent(). Keep it at the beginning if you need resources
        // to initialize other game objects.
        base.Initialize();

        _graphics.PreferredBackBufferWidth = Constants.InitialWindowWidth;
        _graphics.PreferredBackBufferHeight = Constants.InitialWindowHeight;
        _graphics.ApplyChanges();

        _minefield = new(this, 9, 9);
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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            // TODO: Does Monogame provide a function for checking mouse clicks in bounds?
            bool clickInXBounds = mouseState.X >= 0 && mouseState.X <= Constants.InitialWindowWidth;
            bool clickInYBounds = mouseState.Y >= 0 && mouseState.X <= Constants.InitialWindowHeight;
            if (clickInXBounds && clickInYBounds)
            {
                var gridX = (int)MathF.Floor(mouseState.X / Constants.CellSize);
                var gridY = (int)MathF.Floor(mouseState.Y / Constants.CellSize);
                MouseEventArgs eventArgs = new(new Point(gridX, gridY));
                OnRaiseMouseEvent(eventArgs);
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _minefield.Draw(_spriteBatch);
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
