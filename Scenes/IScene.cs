using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper;

public interface IScene
{
    void Enter();
    void Leave();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}