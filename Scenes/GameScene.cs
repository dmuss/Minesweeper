using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if DEBUG
using Microsoft.Xna.Framework.Input;
#endif

namespace Minesweeper;

public class GameScene : BaseScene
{
    private readonly List<Rectangle> _cellSpriteRects;
    private readonly Minefield _minefield;
    private bool _playing = true;

    public GameScene(MSGame game) : base(game)
    {
        _cellSpriteRects = new();
        const byte textureRectSize = 26;
        for (byte y = 0; y < 2; y++)
        {
            for (byte x = 0; x < 7; x++)
            {
                Rectangle rect = new(x * textureRectSize, y * textureRectSize, textureRectSize, textureRectSize);
                _cellSpriteRects.Add(rect);
            }
        }

        _minefield = new();
    }

    public override void Enter()
    {
        _minefield.Reset(MSGame.Difficulty);
        MSGame.SetBackBufferSize(_minefield.Width * Cell.Size, _minefield.Height * Cell.Size);

        base.Enter();
    }

    public override void Leave()
    {
        _playing = true;

        base.Leave();
    }

    public override void Update(GameTime gameTime)
    {
        if (_playing)
        {
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                _playing = false;
                _minefield.Win();
            }
#endif
            // Check for win.
            if (_minefield.RemainingCells == 0)
            {
                Debug.WriteLine("PLAYER WINS!");

                _playing = false;
                _minefield.Win();
            }

            // Update mouse input.
            if (Mouse.Position is Point mousePosition)
            {
                Point minefieldPos = new((int)MathF.Floor(mousePosition.X / Cell.Size),
                                         (int)MathF.Floor(mousePosition.Y / Cell.Size));

                if (Mouse.LeftClick)
                {
                    if (_minefield.RevealCellAtPosition(minefieldPos) is CellState state && state == CellState.Mine)
                    {
                        Debug.WriteLine("PLAYER LOSES!");

                        _playing = false;
                        _minefield.Lose(minefieldPos);
                    }
                }

                if (Mouse.RightClick) { _minefield.FlagCellAtPosition(minefieldPos); }
            }
        }
        else
        {
            if (Mouse.LeftClick)
            {
                MSGame.SceneManager.SwitchScene(SceneManager.Scenes.MainMenu);
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (Cell cell in _minefield.Cells)
        {
            spriteBatch.Draw(texture: MSGame.Sprites,
                             destinationRectangle: cell.Rect,
                             sourceRectangle: _cellSpriteRects[(int)cell.State],
                             color: Color.White);
        }
    }
}