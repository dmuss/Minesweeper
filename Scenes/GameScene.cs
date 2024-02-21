using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly Minefield _mineField;

    public GameScene(in MSGame game)
    {
        game.RaiseMouseEvent += HandleMouseEvent;
        _mineField = new(game, 9, 9);
    }

    public override void Update(GameTime gameTime) { return; }

    public override void Draw(SpriteBatch spriteBatch) { _mineField.Draw(spriteBatch); }

    private void HandleMouseEvent(object sender, MouseEventArgs e)
    {
        _mineField.RevealCell(e.GridLocation.X, e.GridLocation.Y);
    }
}