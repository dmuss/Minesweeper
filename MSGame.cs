using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public enum Difficulty { Easy, Medium, Hard }

public sealed class MSGame : Game
{
    #region Assets 
    public Texture2D Sprites { get => _spriteSheet; }
    public SpriteFont Font { get => _font; }

    private Texture2D _spriteSheet;
    private SpriteFont _font;
    #endregion Assets 

    #region GameSettings
    public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    public int WindowWidth { get => _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth; }
    public int WindowHeight { get => _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight; }
    #endregion GameSettings

    #region Managers
    public SceneManager SceneManager { get => _sceneManager; }

    private SceneManager _sceneManager;
    #endregion MAnagers

    #region Resources
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    #endregion Resources

    #region Public Methods
#pragma warning disable CS8618 // Initialisation in Monogame is typically not done in the Game constructor.
    public MSGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
    }
#pragma warning restore CS8618

    public void SetBackBufferSize(int width, int height)
    {
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();
    }
    #endregion Public Methods

    #region Protected Override Methods
    protected override void Initialize()
    {
        base.Initialize();
        _sceneManager = new(this);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spriteSheet = Content.Load<Texture2D>("Spritesheet");
        _font = Content.Load<SpriteFont>("Silkscreen");
    }

    protected override void Update(GameTime gameTime)
    {
        UpdateInput();
        _sceneManager?.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _sceneManager?.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    #endregion Protected Override Methods

    #region Private Methods
    private void UpdateInput()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

        MouseInput.Update(GraphicsDevice.Viewport.Bounds);
    }
    #endregion Private Methods
}