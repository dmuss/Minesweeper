using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly List<Rectangle> _cellTextureRects;
    private Minefield _minefield;

    public GameScene(in MSGame game) : base(game)
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

        _minefield = new();
    }

    public override void Enter()
    {
        base.Enter();

        _minefield.Reset(MSGame.Difficulty);
        MSGame.SetBackBufferSize(_minefield.GridWidth * Cell.Size, _minefield.GridHeight * Cell.Size);
    }

    public override void Update(GameTime gameTime)
    {
        if (MSGame.MouseInput.Position is Point mousePosition)
        {
            if (MSGame.MouseInput.LeftClick)
            {
                // TODO: Game win/game over notice and reset options.
                if (_minefield.RevealCellAtPosition(mousePosition) == Cell.MineValue || _minefield.PlayerHasWon)
                {
                    _minefield.RevealMines(mousePosition);
                }
            }

            if (MSGame.MouseInput.RightClick)
            {
                _minefield.FlagCellAtPosition(mousePosition);
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
                                         sourceRectangle: _cellTextureRects[cell.Value],
                                         color: Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(texture: MSGame.Sprites,
                                         destinationRectangle: cell.Rect,
                                         sourceRectangle: _cellTextureRects[(byte)cell.State],
                                         color: Color.White);
                    }
                }
            }
        }
    }
}