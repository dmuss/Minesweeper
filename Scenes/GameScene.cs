using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly Minefield _mineField;

    public GameScene(in MSGame game) : base(game)
    {
        _mineField = new(game, 9, 9);
    }

    public override void Update(GameTime gameTime)
    {
        if (MouseInput.LeftClick)
        {
            Point mousePos = MouseInput.Position;
            int gridX = (int)MathF.Floor(mousePos.X / Constants.CellSize);
            int gridY = (int)MathF.Floor(mousePos.Y / Constants.CellSize);
            _mineField.RevealCell(gridX, gridY);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) { _mineField.Draw(spriteBatch); }
}