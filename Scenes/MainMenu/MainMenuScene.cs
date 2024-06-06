using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public class MainMenuScene : BaseScene
{
    private const ushort _windowSize = 750;
    private readonly Rectangle _buttonUpSprite = new(0, 52, 182, 25);
    private readonly Rectangle _buttonDownSprite = new(0, 77, 182, 25);
    private readonly Rectangle _titleSprite = new(0, 102, 182, 21);
    private readonly List<MenuButton> _buttons;

    public MainMenuScene()
    {
        _buttons = new()
        {
            new MenuButton("EASY",
                           new Rectangle(100, 200, 546, 75),
                           Color.White,
                           onPress: () => { StartGame(Difficulty.Easy); }),
            new MenuButton("MEDIUM",
                           new Rectangle(100, 300, 546, 75),
                           Color.Aquamarine,
                           onPress: () => { StartGame(Difficulty.Medium); }),
            new MenuButton("HARD",
                           new Rectangle(100, 400, 546, 75),
                           Color.MistyRose,
                           onPress: () => { StartGame(Difficulty.Hard); }),
            new MenuButton("QUIT",
                           new Rectangle(100, 500, 546, 75),
                           Color.Green,
                           onPress: () => { MSGame.ShouldQuit = true; }),
        };
    }

    public override void Enter()
    {
        MSGame.RequestedWindowSize = new Vector2(_windowSize, _windowSize);

        base.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (MenuButton button in _buttons)
        {
            if (button.IsMouseInButton(MouseInput.Position) && MouseInput.LeftButtonDown)
            {
                button.IsDown = true;
            }
            else
            {
                button.IsDown = false;
            }

            if (button.IsMouseInButton(MouseInput.Position) && MouseInput.LeftButtonClicked)
            {
                button.OnPress();
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

    private static void StartGame(Difficulty difficulty)
    {
        MSGame.Difficulty = difficulty;
        SceneManager.SwitchScene(Scenes.Game);
    }
}