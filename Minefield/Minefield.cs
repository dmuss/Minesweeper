using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public partial class Minefield
{


    private readonly CellGrid _cellGrid;
    private readonly Texture2D _pixel;
    private readonly SpriteFont _font;
    // Indexed by cell values for drawing.
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
        Color.Chartreuse,
        Color.Yellow
    };

    public Minefield(in MSGame game, int minefieldWidth, int minefieldHeight)
    {
        _cellGrid = new CellGrid(minefieldWidth, minefieldHeight);

        _pixel = game.Pixel;
        _font = game.Font;
    }

    public void Update(MouseInputManager mouseInput)
    {
        if (mouseInput.Position is Point mouseScreenPos)
        {
            int mouseCellX = (int)MathF.Floor(mouseScreenPos.X / Constants.CellSize);
            int mouseCellY = (int)MathF.Floor(mouseScreenPos.Y / Constants.CellSize);

            if (mouseInput.LeftClick)
            {
                _cellGrid.RevealCell(new Point(mouseCellX, mouseCellY));
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawCells(spriteBatch);
        DrawGridLines(spriteBatch);
    }

    private void DrawCells(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < _cellGrid.Width; x++)
        {
            for (int y = 0; y < _cellGrid.Height; y++)
            {
                Point cellCoords = new Point(x, y);

                if (_cellGrid.GetCellAtPoint(cellCoords) is Cell cell)
                {
                    if (cell.IsRevealed)
                    {
                        Color cellColour = _cellColours[cell.Value];

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
        for (int x = 0; x < _cellGrid.Width; x++)
        {
            spriteBatch.Draw(_pixel, new Rectangle(x * Constants.CellSize, 0, 1, Constants.RenderHeight), Color.Black * 0.25f);
        }

        for (int y = 0; y < _cellGrid.Height; y++)
        {
            spriteBatch.Draw(_pixel, new Rectangle(0, y * Constants.CellSize, Constants.RenderWidth, 1), Color.Black * 0.25f);
        }
    }
}