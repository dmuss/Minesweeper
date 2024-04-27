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

    public override void Update(GameTime gameTime) { _mineField.Update(MouseInput); }

    public override void Draw(SpriteBatch spriteBatch) { _mineField.Draw(spriteBatch); }
}