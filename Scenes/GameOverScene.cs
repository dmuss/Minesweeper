using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class GameOverScene : BaseScene
{
    public GameOverScene(in MSGame game) : base(game) { }

    public override void Update(GameTime gameTime)
    {
        if (MSGame.MouseInput.LeftClick)
        {
            MSGame.SceneManager.SwitchScene(SceneManager.Scenes.MainMenu);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(MSGame.Pixel, new Rectangle(0, 0, MSGame.WindowWidth, MSGame.WindowHeight), Color.Red);
    }
}