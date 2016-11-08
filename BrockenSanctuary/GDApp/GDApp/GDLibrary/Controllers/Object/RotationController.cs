using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class RotationController : Controller
    {
        private Vector3 rotation;
        private int count = 0;
        public RotationController(string id, ControllerType controllerType, Vector3 rotation)
            : base(id, controllerType)
        {
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            if (parentActor != null)
            {
                parentActor.Transform3D.RotateBy(this.rotation
                    * count * gameTime.ElapsedGameTime.Milliseconds);

                count++;
            }
        }
    }
}

