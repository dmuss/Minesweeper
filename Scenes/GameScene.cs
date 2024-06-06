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
    private readonly List<Rectangle> _cellSpriteSourceRects;
    private readonly Minefield _minefield;
    private bool _playing = true;

    public GameScene()
    {
        _cellSpriteSourceRects = new();
        const byte textureRectSize = 26;
        const byte numCellSpriteRows = 2;
        const byte numCellSpriteColumns = 7;

        // Individual cell sprites are laid out left-to-right, beginning at the top-left of the spritesheet.
        // When a cell is revealed, the underlying integral value of its <c>State</c> is used as an index to
        // _cellSpriteRects so that the proper sprite is displayed. This loop sets up the readonly list.
        for (byte y = 0; y < numCellSpriteRows; y++)
        {
            for (byte x = 0; x < numCellSpriteColumns; x++)
            {
                Rectangle rect = new(x * textureRectSize, y * textureRectSize, textureRectSize, textureRectSize);
                _cellSpriteSourceRects.Add(rect);
            }
        }

        _minefield = new();
    }

    public override void Enter()
    {
        _minefield.Reset(MSGame.Difficulty);

        MSGame.RequestedWindowSize = new(_minefield.GridWidth * Cell.SizeInPixels, _minefield.GridHeight * Cell.SizeInPixels);

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
            Point minefieldPos = new((int)MathF.Floor(MouseInput.Position.X / Cell.SizeInPixels),
                                     (int)MathF.Floor(MouseInput.Position.Y / Cell.SizeInPixels));

            if (MouseInput.LeftButtonClicked)
            {
                if (_minefield.RevealCellAtPosition(minefieldPos) is CellState state && state == CellState.Mine)
                {
                    _playing = false;
                    _minefield.Lose(minefieldPos);
                }
            }

            if (MouseInput.RightButtonClicked) { _minefield.FlagCellAtPosition(minefieldPos); }
        }
        else // Player has either won or lost and the game is over.
        {
            if (MouseInput.LeftButtonClicked || MouseInput.RightButtonClicked)
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
                             sourceRectangle: _cellSpriteSourceRects[(int)cell.State],
                             color: Color.White);
        }
    }
}