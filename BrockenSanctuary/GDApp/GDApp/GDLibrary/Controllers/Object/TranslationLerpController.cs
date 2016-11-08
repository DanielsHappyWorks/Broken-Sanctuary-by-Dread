using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class TranslationLerpController : Controller
    {
        public TranslationLerpController(string id, ControllerType controllerType)
            : base(id, controllerType)
        {

        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;

            if (parentActor != null)
            {
                float sinTime = (float)Math.Sin(MathHelper.ToRadians(100 * (float)gameTime.TotalGameTime.TotalSeconds));

                sinTime *= 0.5f; //-0.5f -> + 0.5f
                sinTime += 0.5f; //0 -> 1

                //calculate the new translation by adding to the original translation
                parentActor.Transform3D.Translation =
                   parentActor.Transform3D.OriginalTransform3D.Translation
                           + sinTime * Vector3.UnitX * 100;
            }

        }
    }
}
