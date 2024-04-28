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
                // TODO: Likely behaviour for a win or lose is to reveal the board. Should there even be win/lose scenes or should it simply
                // be a conditional check for whether the player's won on click to bring them back to the main menu to restart?
                // Or could overlay a restart / difficulty buttons.
                if (MSGame.Minefield.RevealCellAtPosition(mousePosition) == Cell.MineValue)
                {
                    MSGame.Minefield.RevealMines(mousePosition);
                    //MSGame.SceneManager.SwitchScene(SceneManager.Scenes.GameOver);
                }

                if (MSGame.Minefield.PlayerHasWon)
                {
                    Debug.WriteLine("Player wins!");
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
                    switch (cell.State)
                    {
                        case CellState.Hidden:
                            {
                                // TODO: texture indices should probalby not be stored in the cell which isn't concerned with drawing?
                                spriteBatch.Draw(texture: MSGame.CellSheet,
                                                 destinationRectangle: cell.Rect,
                                                 sourceRectangle: MSGame.CellTextureRects[(int)Sprite.Hidden],
                                                 color: Color.White);
                                break;
                            }
                        case CellState.Flagged:
                            {
                                spriteBatch.Draw(texture: MSGame.CellSheet,
                                                 destinationRectangle: cell.Rect,
                                                 sourceRectangle: MSGame.CellTextureRects[Cell.FlaggedValue],
                                                 color: Color.White);
                                break;
                            }
                        case CellState.Question:
                            {
                                spriteBatch.Draw(texture: MSGame.CellSheet,
                                                 destinationRectangle: cell.Rect,
                                                 sourceRectangle: MSGame.CellTextureRects[Cell.QuestionValue],
                                                 color: Color.White);
                                break;
                            }
                        case CellState.Revealed:
                            {
                                spriteBatch.Draw(texture: MSGame.CellSheet,
                                                 destinationRectangle: cell.Rect,
                                                 sourceRectangle: MSGame.CellTextureRects[cell.Value],
                                                 color: Color.White);
                                break;
                            }
                        default:
                            break;

                    }
                }
            }
        }
    }
}
