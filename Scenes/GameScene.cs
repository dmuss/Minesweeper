using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    public GameScene(in MSGame game) : base(game) { }

    public override void Enter()
    {
        MSGame.Minefield.Reset(MSGame.Difficulty);
        base.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        if (MSGame.MouseInput.Position is Point mousePosition)
        {
            if (MSGame.MouseInput.LeftClick)
            {
                // TODO: Stop taking mouse input in minefield and provide reset options.
                if (MSGame.Minefield.RevealCellAtPosition(mousePosition) == Cell.MineValue)
                {
                    MSGame.Minefield.RevealMines(mousePosition);
                }

                if (MSGame.Minefield.PlayerHasWon)
                {
                    Debug.WriteLine("Player wins!");
                    MSGame.Minefield.RevealMines(mousePosition);
                }
            }

            if (MSGame.MouseInput.RightClick)
            {
                MSGame.Minefield.FlagCellAtPosition(mousePosition);
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch) { DrawCells(spriteBatch); }

    private void DrawCells(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < MSGame.Minefield.GridWidth; x++)
        {
            for (int y = 0; y < MSGame.Minefield.GridHeight; y++)
            {
                Point cellCoords = new(x, y);

                if (MSGame.Minefield.GetCellAtPoint(cellCoords) is Cell cell)
                {
                    // If cell is revealed, it's value indexes the proper source rectangle for the sprite,
                    // otherwise use the underlying integral value of the state to get the proper sprite.
                    if (cell.State == CellState.Revealed)
                    {
                        spriteBatch.Draw(texture: MSGame.CellSheet,
                                         destinationRectangle: cell.Rect,
                                         sourceRectangle: MSGame.CellTextureRects[cell.Value],
                                         color: Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(texture: MSGame.CellSheet,
                                         destinationRectangle: cell.Rect,
                                         sourceRectangle: MSGame.CellTextureRects[(byte)cell.State],
                                         color: Color.White);
                    }
                }
            }
        }
    }
}