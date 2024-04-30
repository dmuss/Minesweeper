using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class MouseInputManager
{
    public Point Position { get => _position; }
    public bool LeftClick { get => _leftButtonClick; }
    public bool LeftDown { get => _leftButtonDown; }
    public bool RightClick { get => _rightButtonClick; }
    public bool RightDown { get => _rightButtonDown; }

    private MouseState _oldState;
    private MouseState _currentState;

    private Point _position;
    private bool _leftButtonDown;
    private bool _leftButtonClick;
    private bool _rightButtonDown;
    private bool _rightButtonClick;

    public void Update(Rectangle screenBounds)
    {
        _currentState = Mouse.GetState();

        if (MouseInScreenBounds(screenBounds))
        {
            _leftButtonDown = _currentState.LeftButton == ButtonState.Pressed;
            _leftButtonClick = _currentState.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed;

            _rightButtonDown = _currentState.RightButton == ButtonState.Pressed;
            _rightButtonClick = _currentState.RightButton == ButtonState.Released && _oldState.RightButton == ButtonState.Pressed;

            _position = _currentState.Position;
        }

        _oldState = _currentState;
    }

    public void Reset() { _oldState = Mouse.GetState(); }

    private bool MouseInScreenBounds(Rectangle screenBounds)
    {
        bool inXBounds = _currentState.X >= screenBounds.X && _currentState.X <= (screenBounds.X + screenBounds.Width);
        bool inYBounds = _currentState.Y >= screenBounds.Y && _currentState.Y <= (screenBounds.Y + screenBounds.Height);

        return inXBounds && inYBounds;
    }
}