using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public interface IController
    {
        void Update(GameTime gameTime, IActor actor);
        string GetID();
    }
}
