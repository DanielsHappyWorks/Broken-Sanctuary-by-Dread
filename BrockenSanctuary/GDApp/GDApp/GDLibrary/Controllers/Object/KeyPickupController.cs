using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class KeyPickupController : Controller
    {
        private Vector3 rotation/*, translation*/;
        private int count = 0;
        public KeyPickupController(string id, ControllerType controllerType,/* Vector3 translation,*/
             Vector3 rotation)
            : base(id, controllerType)
        {
            this.rotation = rotation;
           // this.translation = translation;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            if (parentActor != null)
            {
                /*parentActor.Transform3D.TranslateBy(this.translation 
                    * count * gameTime.ElapsedGameTime.Milliseconds);*/
                parentActor.Transform3D.RotateBy(this.rotation
                    * count * gameTime.ElapsedGameTime.Milliseconds);

                count++;
            }
        }
    }
}

