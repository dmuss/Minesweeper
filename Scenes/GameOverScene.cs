using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameOverScene : BaseScene
{
    private Texture2D _pixel;

    public GameOverScene(in MSGame game) : base(game)
    {
        _pixel = game.Pixel;
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: Proper buttons and settings for difficulty, etc.
        if (MouseInput.LeftClick)
        {
            OnChangeScene(new SceneChangeArgs(SceneManager.Scenes.MainMenu));
        }

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, new Rectangle(0, 0, Constants.InitialWindowWidth, Constants.InitialWindowHeight), Color.Red);
    }
}