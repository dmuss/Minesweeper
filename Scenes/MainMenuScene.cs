using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class MainMenuScene : BaseScene
{
    private const ushort _windowSize = 750;
    private Rectangle _buttonUpSprite = new(0, 52, 182, 25);
    private Rectangle _buttonDownSprite = new(0, 77, 182, 25);
    private Rectangle _titleSprite = new(0, 102, 182, 21);
    private List<MenuButton> _buttons;

    public MainMenuScene(MSGame game) : base(game)
    {
        _buttons = new()
        {
            new MenuButton("EASY",
                           new Rectangle(100, 200, 546, 75),
                           Color.White,
                           onPress: () => { MSGame.Difficulty = Difficulty.Easy; MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game); }),
            new MenuButton("MEDIUM",
                           new Rectangle(100, 300, 546, 75),
                           Color.Aquamarine,
                           onPress: () => { MSGame.Difficulty = Difficulty.Medium; MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game); }),
            new MenuButton("HARD",
                           new Rectangle(100, 400, 546, 75),
                           Color.MistyRose,
                           onPress: () => { MSGame.Difficulty = Difficulty.Hard; MSGame.SceneManager.SwitchScene(SceneManager.Scenes.Game); }),
            new MenuButton("QUIT",
                           new Rectangle(100, 500, 546, 75),
                           Color.Green,
                           onPress: () => { MSGame.Exit(); }),
        };
    }

    public override void Enter()
    {
        MSGame.SetBackBufferSize(_windowSize, _windowSize);

        base.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        if (Mouse.Position is Point mousePosition)
        {
            foreach (MenuButton button in _buttons)
            {
                if (button.MouseInButton(mousePosition) && Mouse.LeftDown)
                {
                    button.IsDown = true;
                }
                else
                {
                    button.IsDown = false;
                }

                if (button.MouseInButton(mousePosition) && Mouse.LeftClick)
                {
                    button.OnPress();
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture: MSGame.Sprites,
                         destinationRectangle: new Rectangle(100, 100, 546, 75),
                         sourceRectangle: _titleSprite,
                         Color.White);

        foreach (MenuButton button in _buttons)
        {
            spriteBatch.Draw(texture: MSGame.Sprites,
                             destinationRectangle: button.Rect,
                             sourceRectangle: button.IsDown ? _buttonDownSprite : _buttonUpSprite,
                             color: button.Colour);

            Vector2 halflabelSize = MSGame.Font.MeasureString(button.Label) / 2;
            spriteBatch.DrawString(spriteFont: MSGame.Font,
                                   text: button.Label,
                                   position: new Vector2(button.Rect.Center.X - halflabelSize.X, button.Rect.Y + 3),
                                   color: button.IsDown ? Color.Black * 0.7f : Color.Black * 0.6f);
        }
    }
}