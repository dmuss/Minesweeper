using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class MainMenuScene : BaseScene
{
    private Texture2D _pixel;
    private SpriteFont _font;

    public MainMenuScene(MSGame game)
    {
        game.RaiseMouseEvent += HandleMouseEvent;
        _pixel = game.Pixel;
        _font = game.Font;
    }

    public override void Update(GameTime gameTime) { /* throw new System.NotImplementedException(); */ }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, new Rectangle(100, 100, 400, 100), Color.White);
        spriteBatch.Draw(_pixel, new Rectangle(100, 300, 400, 100), Color.Blue);
        spriteBatch.Draw(_pixel, new Rectangle(100, 500, 400, 100), Color.Red);
    }

    protected override void OnChangeScene(SceneChangeArgs e) { base.OnChangeScene(e); }

    private void HandleMouseEvent(object sender, MouseEventArgs args)
    {
        OnChangeScene(new SceneChangeArgs(SceneManager.Scenes.Game));
    }
}