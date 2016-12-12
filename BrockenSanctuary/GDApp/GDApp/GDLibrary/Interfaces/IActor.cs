using Microsoft.Xna.Framework;

//base class from which all drawn, collidable, 
//non-collidable, trigger volumes, and camera inherit
namespace GDLibrary
{
    public interface IActor
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);

        ActorType GetActorType();
        string GetID();
        bool Remove();
        float GetAlpha();
    }
}
