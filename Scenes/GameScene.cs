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
                if (MSGame.Minefield.RevealCell(mousePosition) == Cell.MineValue)
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
                        spriteBatch.DrawString(MSGame.Font,
                                               cell.Value.ToString(),
                                               new Vector2(cell.Rect.X + (Cell.Size / 2),
                                                           cell.Rect.Y + (Cell.Size / 2)),
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