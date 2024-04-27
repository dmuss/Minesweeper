using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly Minefield _mineField;
    private readonly Texture2D _pixel;
    private readonly SpriteFont _font;

    // TODO: Pass difficulty around while changing scenes.
    public GameScene(in MSGame game) : base(game)
    {
        _mineField = new(9, 9);
        _pixel = game.Pixel;
        _font = game.Font;
    }

    public override void Update(GameTime gameTime)
    {
        if (MouseInput.Position is Point mousePosition)
        {
            if (MouseInput.LeftClick)
            {
                if (_mineField.RevealCell(mousePosition) == Cell.MineValue)
                {
                    OnChangeScene(new SceneChangeArgs(SceneManager.Scenes.GameOver));
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        DrawCells(spriteBatch);
        DrawGridLines(spriteBatch);
    }

    private void DrawCells(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < _mineField.GridWidth; x++)
        {
            for (int y = 0; y < _mineField.GridHeight; y++)
            {
                Point cellCoords = new(x, y);

                if (_mineField.GetCellAtPoint(cellCoords) is Cell cell)
                {
                    if (cell.IsRevealed)
                    {

                        Color cellColour = _mineField.CellColours[cell.Value];

                        spriteBatch.Draw(_pixel, cell.Rect, cellColour);
                        spriteBatch.DrawString(_font,
                                               cell.Value.ToString(),
                                               new Vector2(cell.Rect.X + Constants.CellSize / 2,
                                                           cell.Rect.Y + Constants.CellSize / 2),
                                               Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(_pixel, cell.Rect, Color.White);
                    }
                }
            }
        }
    }

    private void DrawGridLines(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < _mineField.GridWidth; x++)
        {
            spriteBatch.Draw(_pixel, new Rectangle(x * Constants.CellSize, 0, 1, Constants.InitialWindowHeight), Color.Black * 0.25f);
        }

        for (int y = 0; y < _mineField.GridHeight; y++)
        {
            spriteBatch.Draw(_pixel, new Rectangle(0, y * Constants.CellSize, Constants.InitialWindowWidth, 1), Color.Black * 0.25f);
        }

    }
}