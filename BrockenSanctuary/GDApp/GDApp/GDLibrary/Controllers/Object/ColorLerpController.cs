using Microsoft.Xna.Framework;
using System;
namespace GDLibrary
{
    public class ColorLerpController : Controller
    {
        private Color startColor;
        private Color endColor;
        private float lerpSpeed;

        public ColorLerpController(string id, 
            ControllerType controllerType, 
            Color startColor, Color endColor, float lerpSpeed)
            : base(id, controllerType)
        {
            this.startColor = startColor;
            this.endColor = endColor;
            this.lerpSpeed = lerpSpeed;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            DrawnActor3D parentActor = actor as DrawnActor3D;

            if (parentActor != null)
            {
                float time = (float)gameTime.TotalGameTime.TotalSeconds;
                time %= 360; //0 - > 359 degrees

                float lerpFactor
                    = (float)Math.Sin(MathHelper.ToRadians(
                    this.lerpSpeed * time));

                lerpFactor *= 0.5f; //scale to 0->1
                lerpFactor += 0.5f;

                parentActor.Color = MathUtility.Lerp(this.startColor,
                            this.endColor, lerpFactor);
            }
        }
    }
}

