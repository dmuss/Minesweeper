using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class MainMenuScene : BaseScene
{
    private const ushort _windowSize = 750;

    public MainMenuScene(in MSGame game) : base(game) { }

    public override void Enter()
    {
        base.Enter();

        MSGame.SetBackBufferSize(_windowSize, _windowSize);
    }

    public override void Update(GameTime gameTime)
    {
        if (MSGame.MouseInput.LeftClick)
        {
            MSGame.Difficulty = Difficulty.Easy;
            MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game);
        }

        if (MSGame.MouseInput.RightClick)
        {
            MSGame.Difficulty = Difficulty.Medium;
            MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game);
        }

        if (MSGame.MouseInput.MiddleClick)
        {
            MSGame.Difficulty = Difficulty.Hard;
            MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(MSGame.Pixel, new Rectangle(100, 100, 400, 100), Color.White);
        spriteBatch.Draw(MSGame.Pixel, new Rectangle(100, 300, 400, 100), Color.Blue);
        spriteBatch.Draw(MSGame.Pixel, new Rectangle(100, 500, 400, 100), Color.Red);
    }
}