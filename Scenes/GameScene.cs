using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if DEBUG
using Microsoft.Xna.Framework.Input;
#endif

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly List<Rectangle> _cellSpriteRects;
    private readonly Minefield _minefield;
    private bool _playing = true;
    private bool _playerHasWon = false;

    public GameScene(MSGame game) : base(game)
    {
        _cellSpriteRects = new();
        const byte textureRectSize = 26;
        for (byte y = 0; y < 2; y++)
        {
            for (byte x = 0; x < 7; x++)
            {
                Rectangle rect = new(x * textureRectSize, y * textureRectSize, textureRectSize, textureRectSize);
                _cellSpriteRects.Add(rect);
            }
        }

        _minefield = new();
    }

    public override void Enter()
    {
        _minefield.Reset(MSGame.Difficulty);
        MSGame.SetBackBufferSize(_minefield.GridWidth * Cell.Size, _minefield.GridHeight * Cell.Size);

        base.Enter();
    }

    public override void Leave()
    {
        _playing = true;
        _playerHasWon = false;

        base.Leave();
    }

    public override void Update(GameTime gameTime)
    {
        if (_playing)
        {
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                _playerHasWon = true;
                _playing = false;
                _minefield.Reveal();
            }
#endif
            // Check for win.
            if (_minefield.RemainingCells == 0)
            {
                Debug.WriteLine("PLAYER WINS!");

                _playerHasWon = true;
                _playing = false;
                _minefield.RevealMines(null, _playerHasWon);
            }

            // Update mouse input.
            if (Mouse.Position is Point mousePosition)
            {
                if (Mouse.LeftClick)
                {
                    if (_minefield.RevealCellAtPosition(mousePosition) == Cell.MineValue)
                    {
                        Debug.WriteLine("PLAYER LOSES!");

                        _playing = false;
                        _minefield.RevealMines(mousePosition, _playerHasWon);
                    }
                }

                if (Mouse.RightClick)
                {
                    _minefield.FlagCellAtPosition(mousePosition);
                }
            }
        }
        else
        {
            if (Mouse.LeftClick)
            {
                MSGame.SceneManager.SwitchScene(SceneManager.Scenes.MainMenu);
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < _minefield.GridWidth; x++)
        {
            for (int y = 0; y < _minefield.GridHeight; y++)
            {
                Point cellCoords = new(x, y);
                if (_minefield.GetCellAtPoint(cellCoords) is Cell cell)
                {
                    // If cell is revealed, its value indexes the proper source rectangle for the sprite,
                    // otherwise use the underlying integral value of the state enum.
                    if (cell.State == CellState.Revealed)
                    {
                        spriteBatch.Draw(texture: MSGame.Sprites,
                                         destinationRectangle: cell.Rect,
                                         sourceRectangle: _cellSpriteRects[cell.Value],
                                         color: Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(texture: MSGame.Sprites,
                                         destinationRectangle: cell.Rect,
                                         sourceRectangle: _cellSpriteRects[(byte)cell.State],
                                         color: Color.White);
                    }
                }
            }
        }

        //if (!_playing)
        //{
        //    string textString = _playerHasWon ? "YOU WON" : "YOU LOST";
        //    Vector2 halfTextSize = MSGame.Font.MeasureString(textString) / 2;
        //    Vector2 viewportCenter = new(MSGame.GraphicsDevice.Viewport.Width / 2, MSGame.GraphicsDevice.Viewport.Height / 2);
        //    spriteBatch.DrawString(MSGame.Font,
        //                           textString,
        //                           new Vector2(viewportCenter.X - halfTextSize.X, viewportCenter.Y - halfTextSize.Y),
        //                           Color.Black);
        //}
    }
}