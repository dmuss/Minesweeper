using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public enum Difficulty { Easy, Medium, Hard }

public class MSGame : Game
{

    public Texture2D Pixel { get => _pixel; }
    public Texture2D CellSheet { get => _cellSheet; }
    public List<Rectangle> CellTextureRects { get => _cellTextureRects; }
    public MouseInputManager MouseInput { get => _mouseInput; }
    public SceneManager SceneManager { get => _sceneManager; }
    public Minefield Minefield { get => _mineField; }
    public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    public int WindowWidth { get => _graphics.PreferredBackBufferWidth; }
    public int WindowHeight { get => _graphics.PreferredBackBufferHeight; }

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Texture2D _cellSheet;
    private SceneManager _sceneManager;
    private MouseInputManager _mouseInput;
    private Minefield _mineField;

    private List<Rectangle> _cellTextureRects;

#pragma warning disable CS8618 // Fields are not initialized in the Game constructor.
    public MSGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
#pragma warning restore CS8618

    public void SetBackBufferSize(int width, int height)
    {
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        base.Initialize();

        InitializeCellTextureRects();

        _mineField = new();
        SetBackBufferSize(_mineField.GridWidth * Cell.Size, _mineField.GridHeight * Cell.Size);

        _mouseInput = new();
        _sceneManager = new(this);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new(GraphicsDevice, 1, 1);
        _pixel.SetData(new Color[] { Color.White });

        _cellSheet = Content.Load<Texture2D>("CellSheet");
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

    private void UpdateInput()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

        _mouseInput.Update(GraphicsDevice.Viewport.Bounds);
    }

    private void InitializeCellTextureRects()
    {
        _cellTextureRects = new();
        const byte textureRectSize = 26;
        for (byte y = 0; y < 2; y++)
        {
            for (byte x = 0; x < 7; x++)
            {
                Rectangle rect = new(x * textureRectSize, y * textureRectSize, textureRectSize, textureRectSize);
                _cellTextureRects.Add(rect);
            }
        }
    }
}