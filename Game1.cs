using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private RenderTarget2D _gameRenderTarget;
    private Vector2 _renderOffset = new(0, 0);
    private float _renderScale = Constants.InitialWindowWidth / Constants.RenderWidth; // initial window hieght / renderheight

    private Texture2D _pixel;
    private SpriteFont _font;

    private ushort _timer = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 600;
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.ApplyChanges();

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnWindowClientChange;

        _gameRenderTarget = new(GraphicsDevice, Constants.RenderWidth, Constants.RenderHeight);

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
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        DrawToRenderTarget(gameTime);

        Matrix renderTransform =
            Matrix.CreateScale(_renderScale) *
            Matrix.CreateTranslation(_renderOffset.X, _renderOffset.Y, 0);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: renderTransform);
        _spriteBatch.Draw(_gameRenderTarget, Vector2.Zero, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawToRenderTarget(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_gameRenderTarget);
        GraphicsDevice.Clear(Color.AliceBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_pixel, new Rectangle(10, 10, 50, 50), Color.White);
        _spriteBatch.DrawString(_font, gameTime.TotalGameTime.Seconds.ToString(), new Vector2(40, 20), Color.Black);
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
    }

    private void OnWindowClientChange(object sender, EventArgs args)
    {
        float windowAspectRatio = (float)Window.ClientBounds.Width / Window.ClientBounds.Height;
        if (windowAspectRatio < Constants.AspectRatio)
        {
            // Letterbox.
            _renderScale = (float)Window.ClientBounds.Width / Constants.RenderWidth;
            _renderOffset.Y = (Window.ClientBounds.Height - Constants.RenderHeight * _renderScale) / 2.0f;
            _renderOffset.X = 0;
        }
        else
        {
            // Pillarbox.
            _renderScale = (float)Window.ClientBounds.Height / Constants.RenderHeight;
            _renderOffset.X = (Window.ClientBounds.Width - Constants.RenderWidth * _renderScale) / 2.0f;
            _renderOffset.Y = 0;
        }
    }
}
