using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly Minefield _mineField;

    public GameScene(in MSGame game)
    {
        game.RaiseMouseEvent += OnMouseEvent;
        _mineField = new(game, this, 9, 9);
    }

    public override void Update(GameTime gameTime) { return; }

    public override void Draw(SpriteBatch spriteBatch) { _mineField.Draw(spriteBatch); }

    protected override void OnMouseEvent(object sender, MouseEventArgs e)
    {
        if (IsCurrentScene) { base.OnMouseEvent(sender, e); }
    }

}