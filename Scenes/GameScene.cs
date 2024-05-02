using System;
using System.Collections.Generic;
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

    public GameScene()
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

        MSGame.RequestedWindowSize = new(_minefield.Width * Cell.Size, _minefield.Height * Cell.Size);

        base.Enter();
    }

    public override void Leave()
    {
        _playing = true;

        base.Leave();
    }

    public override void Update(GameTime gameTime)
    {
        if (_minefield.RemainingCellsToWin == 0)
        {
            _playing = false;
            _minefield.Win();
        }

#if DEBUG
        if (Keyboard.GetState().IsKeyDown(Keys.F12))
        {
            _playing = false;
            _minefield.Win();
        }
#endif

        if (_playing)
        {
            Point minefieldPos = new((int)MathF.Floor(MouseInput.Position.X / Cell.Size),
                                     (int)MathF.Floor(MouseInput.Position.Y / Cell.Size));

            if (MouseInput.LeftClick)
            {
                if (_minefield.RevealCellAtPosition(minefieldPos) is CellState state && state == CellState.Mine)
                {
                    _playing = false;
                    _minefield.Lose(minefieldPos);
                }
            }

            if (MouseInput.RightClick) { _minefield.FlagCellAtPosition(minefieldPos); }
        }
        else
        {
            if (MouseInput.LeftClick || MouseInput.RightClick)
            {
                SceneManager.SwitchScene(Scenes.MainMenu);
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