using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class PendulumController : Controller
    {
        public PendulumController(string id, ControllerType controllerType)
            : base(id, controllerType)
        {

        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;

            if (parentActor != null)
            {
                if (game.interactedWithClock) { 
                float sinTime = (float)Math.Sin(MathHelper.ToRadians(60* (float)gameTime.TotalGameTime.TotalSeconds));

                sinTime *= -.5f; //-0.5f -> + 0.5f
                sinTime += .25f;//0 -> 1

                //calculate the new translation by adding to the original translation
                parentActor.Transform3D.Rotation =
                   parentActor.Transform3D.OriginalTransform3D.Rotation
                           + sinTime * Vector3.UnitX * 40;}
                else
                {
                    parentActor.Transform3D.Rotation = Vector3.Zero;
                }
            }

        }
    }
}
