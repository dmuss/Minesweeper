using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

/// <summary>
/// Global static mouse input controller.
/// </summary>
public static class MouseInput
{
    /// <summary>
    /// The current screen position of the mouse, in pixels.
    /// </summary>
    public static Point Position { get => _position; }
    public static bool LeftButtonClicked { get => _leftButtonClick; }
    public static bool LeftButtonDown { get => _leftButtonDown; }
    public static bool RightButtonClicked { get => _rightButtonClick; }
    public static bool RightButtonDown { get => _rightButtonDown; }

    private static MouseState _oldState;
    private static MouseState _currentState;

    private static Point _position;
    private static bool _leftButtonDown;
    private static bool _leftButtonClick;
    private static bool _rightButtonDown;
    private static bool _rightButtonClick;

    /// <summary>
    /// Updates the state of the mouse as long as it is within the provided screen bounds. When the mouse is outside
    /// window bounds, state will reflect when it was last in the window.
    /// </summary>
    /// <param name="screenBounds">
    /// The rectangle representing the current screen bounds, in pixels.
    /// </param>
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