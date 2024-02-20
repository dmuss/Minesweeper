using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public partial class Minefield
{
    #region Fields
    private readonly Color[] _cellColours =
    {
        Color.Gray,
        Color.Blue,
        Color.Green,
        Color.Red,
        Color.Navy,
        Color.Maroon,
        Color.Teal,
        Color.Purple,
        Color.Black,
        Color.Yellow
    };
    private readonly Grid _grid;
    private readonly Texture2D _pixel;
    private readonly SpriteFont _font;
    #endregion

    #region Constructors
    // TODO: Passing in both the game and scene here feels inelegant.
    public Minefield(MSGame game, BaseScene scene, int width, int height)
    {
        _grid = new(width, height/*TODO: pass difficulty from game*/);

        _pixel = game.Pixel;
        _font = game.Font;

        scene.MouseEvent += HandleMouseEvent;
    }
    #endregion

    #region Methods
    public void Draw(SpriteBatch _spriteBatch)
    {
        // Draw cells.
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                bool isRevealed = _grid.IsRevealed(x, y);
                Color cellColour = _cellColours[_grid.ValueAt(x, y)];
                Rectangle cellRect = new(Constants.CellSize * x,
                                         Constants.CellSize * y,
                                         Constants.CellSize,
                                         Constants.CellSize);
                if (!isRevealed)
                {
                    _spriteBatch.Draw(_pixel, cellRect, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_pixel, cellRect, cellColour);
                    string cellValue = _grid.ValueAt(x, y).ToString();
                    _spriteBatch.DrawString(_font,
                                            cellValue,
                                            new Vector2(cellRect.X + Constants.CellSize / 2,
                                                        cellRect.Y + Constants.CellSize / 2),
                                            Color.Black);
                }
            }
        }

        // Draw grid lines.
        for (int x = 0; x < _grid.Width; x++)
        {
            _spriteBatch.Draw(_pixel,
                              new Rectangle(x * Constants.CellSize, 0, 1, Constants.RenderHeight),
                              Color.Black * 0.25f);
        }

        for (int y = 0; y < _grid.Height; y++)
        {
            _spriteBatch.Draw(_pixel,
                              new Rectangle(0, y * Constants.CellSize, Constants.RenderWidth, 1),
                              Color.Black * 0.25f);
        }
    }

    private void HandleMouseEvent(object sender, MouseEventArgs args)
    {
        // TODO: Click down changes to a "down" tile, release reveals.
        // TODO: Right click flags cell.
        _grid.RevealCell(args.GridLocation.X, args.GridLocation.Y);
    }
    #endregion
}