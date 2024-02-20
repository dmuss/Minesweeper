using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class MainMenuScene : BaseScene
{
    private Texture2D _pixel;
    private SpriteFont _font;

    public MainMenuScene(MSGame game)
    {
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
}