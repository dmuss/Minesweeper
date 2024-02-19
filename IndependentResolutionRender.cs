// TODO: Reimplement resolution-independent rendering.
// This file contains the code snippets required to render a game scene to a target and scale
// that target to the window bounds with pillar and letterboxing, as necessary.


////////// Fields
// private RenderTarget2D _gameRenderTarget;
// private Vector2 _renderOffset = new(0, 0);
// private float _renderScale = Constants.InitialWindowWidth / Constants.RenderWidth;

////////// Initialize()
// Window.AllowUserResizing = true;
// Window.ClientSizeChanged += OnWindowClientChange;

// _gameRenderTarget = new(GraphicsDevice, Constants.RenderWidth, Constants.RenderHeight);

/////////// Draw()
// DrawToRenderTarget(gameTime);

// Matrix renderTransform =
//     Matrix.CreateScale(_renderScale) *
//     Matrix.CreateTranslation(_renderOffset.X, _renderOffset.Y, 0);

// _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: renderTransform);
// _spriteBatch.Draw(_gameRenderTarget, Vector2.Zero, Color.White);
// _spriteBatch.End();

////////// private void DrawToRenderTarget(GameTime gameTime)
// {
//     GraphicsDevice.SetRenderTarget(_gameRenderTarget);
//     GraphicsDevice.Clear(Color.CornflowerBlue);

//     _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
//     for (var y = 0; y < _cells.GetLength(0); y++)
//     {
//         for (var x = 0; x < _cells.GetLength(1); x++)
//         {
//             string cellLocation = x + "," + y;
//             Rectangle cellRect = new(Constants.CellSize * x, Constants.CellSize * y, Constants.CellSize, Constants.CellSize);
//             _spriteBatch.Draw(_pixel, cellRect, Color.White);
//             _spriteBatch.DrawString(_font, cellLocation, new Vector2(cellRect.X, cellRect.Y), Color.Black);
//         }
//     }
//     _spriteBatch.End();

//     GraphicsDevice.SetRenderTarget(null);
// }

//////////private void OnWindowClientChange(object sender, EventArgs args)
//{
//    float windowAspectRatio = (float)Window.ClientBounds.Width / Window.ClientBounds.Height;
//    if (windowAspectRatio < Constants.AspectRatio)
//    {
//        // Letterbox.
//        _renderScale = (float)Window.ClientBounds.Width / Constants.RenderWidth;
//        _renderOffset.Y = (Window.ClientBounds.Height - Constants.RenderHeight * _renderScale) / 2.0f;
//        _renderOffset.X = 0;
//        Console.WriteLine($"Render Offset ({_renderOffset.X}, {_renderOffset.Y})");
//    }
//    else
//    {
//        // Pillarbox.
//        _renderScale = (float)Window.ClientBounds.Height / Constants.RenderHeight;
//        _renderOffset.X = (Window.ClientBounds.Width - Constants.RenderWidth * _renderScale) / 2.0f;
//        _renderOffset.Y = 0;
//        Console.WriteLine($"Render Offset ({_renderOffset.X}, {_renderOffset.Y})");
//    }
//}