using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public enum Difficulty { Easy, Medium, Hard }

public sealed class MSGame : Game
{
    public static bool ShouldQuit { get; set; }
    public static Vector2 RequestedWindowSize { get; set; }
    public static Difficulty Difficulty { get; set; } = Difficulty.Easy;
    public static Texture2D Sprites { get => _spriteSheet; }
    public static SpriteFont Font { get => _font; }

    private static Texture2D _spriteSheet; // Initialised in LoadContent()
    private static SpriteFont _font;       // Initialised in LoadContent()

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; // Initialised in LoadContent()

    public MSGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spriteSheet = Content.Load<Texture2D>("Spritesheet");
        _font = Content.Load<SpriteFont>("Silkscreen");
    }

    protected override void Update(GameTime gameTime)
    {
        if (ShouldQuit) { Exit(); }

        if (RequestedWindowSize.X != GraphicsDevice.Viewport.Width || RequestedWindowSize.Y != GraphicsDevice.Viewport.Height)
        {
            _graphics.PreferredBackBufferWidth = (int)RequestedWindowSize.X;
            _graphics.PreferredBackBufferHeight = (int)RequestedWindowSize.Y;
            _graphics.ApplyChanges();
        }

        UpdateInput();

        SceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SceneManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateInput()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) { ShouldQuit = true; }

        MouseInput.Update(GraphicsDevice.Viewport.Bounds);
    }
}