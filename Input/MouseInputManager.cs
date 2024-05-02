using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public static class MouseInput
{
    public static Point Position { get => _position; }
    public static bool LeftClick { get => _leftButtonClick; }
    public static bool LeftDown { get => _leftButtonDown; }
    public static bool RightClick { get => _rightButtonClick; }
    public static bool RightDown { get => _rightButtonDown; }

    private static MouseState _oldState;
    private static MouseState _currentState;

    private static Point _position;
    private static bool _leftButtonDown;
    private static bool _leftButtonClick;
    private static bool _rightButtonDown;
    private static bool _rightButtonClick;

    public static void Update(Rectangle screenBounds)
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

    public static void Reset() { _oldState = Mouse.GetState(); }

    private static bool MouseInScreenBounds(Rectangle screenBounds)
    {
        bool inXBounds = _currentState.X >= screenBounds.X && _currentState.X <= (screenBounds.X + screenBounds.Width);
        bool inYBounds = _currentState.Y >= screenBounds.Y && _currentState.Y <= (screenBounds.Y + screenBounds.Height);

        return inXBounds && inYBounds;
    }
}