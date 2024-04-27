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
                if (MSGame.Minefield.RevealCell(mousePosition) == Cell.MineValue)
                {
                    MSGame.SceneManager.SwitchScene(SceneManager.Scenes.GameOver);
                }

                if (MSGame.Minefield.PlayerHasWon())
                {
                    Debug.WriteLine("Player wins!");
                    // TODO: MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Win); 
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
        for (int x = 0; x < MSGame.Minefield.GridWidth; x++)
        {
            for (int y = 0; y < MSGame.Minefield.GridHeight; y++)
            {
                Point cellCoords = new(x, y);

                if (MSGame.Minefield.GetCellAtPoint(cellCoords) is Cell cell)
                {
                    if (cell.IsRevealed)
                    {
                        Color cellColour = MSGame.Minefield.CellColours[cell.Value];

                        spriteBatch.Draw(MSGame.Pixel, cell.Rect, cellColour);

                        string cellText = cell.Value.ToString();
                        Vector2 halfTextSize = MSGame.Font.MeasureString(cellText) / 2;
                        Vector2 cellCenter = new(cell.Rect.X + Cell.HalfSize, cell.Rect.Y + Cell.HalfSize);
                        Vector2 textPosition = new(cellCenter.X - halfTextSize.X, cellCenter.Y - halfTextSize.Y);
                        spriteBatch.DrawString(MSGame.Font,
                                               cellText,
                                               textPosition,
                                               Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(MSGame.Pixel, cell.Rect, Color.White);
                    }
                }
            }
        }
    }

    private void DrawGridLines(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < MSGame.Minefield.GridWidth; x++)
        {
            spriteBatch.Draw(MSGame.Pixel,
                             new Rectangle(x * Cell.Size,
                                           0,
                                           1,
                                           MSGame.WindowHeight),
                             Color.Black * 0.25f);
        }

        for (int y = 0; y < MSGame.Minefield.GridHeight; y++)
        {
            spriteBatch.Draw(MSGame.Pixel,
                             new Rectangle(0,
                                           y * Cell.Size,
                                           MSGame.WindowWidth,
                                           1),
                             Color.Black * 0.25f);
        }

    }
}