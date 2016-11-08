using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class UserInputController : Controller
    {
        #region Variables
        private Keys[] moveKeys;
        private float moveSpeed, strafeSpeed, rotationSpeed;
        #endregion

        #region Properties
        public Keys[] MoveKeys
        {
            get
            {
                return this.moveKeys;
            }
            set
            {
                this.moveKeys = value;
            }
        }
        public float MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }
            set
            {
                this.moveSpeed = value;
            }
        }
        public float StrafeSpeed
        {
            get
            {
                return this.strafeSpeed;
            }
            set
            {
                this.strafeSpeed = value;
            }
        }
        public float RotationSpeed
        {
            get
            {
                return this.rotationSpeed;
            }
            set
            {
                this.rotationSpeed = value;
            }
        }
        #endregion
        
        public UserInputController(string id,
            ControllerType controllerType, Keys[] moveKeys,
            float moveSpeed, float strafeSpeed, float rotationSpeed)
            : base(id, controllerType)
        {
            this.moveKeys = moveKeys;
            this.moveSpeed = moveSpeed;
            this.strafeSpeed = strafeSpeed;
            this.rotationSpeed = rotationSpeed; 
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            HandleMouseInput(gameTime, parentActor);
            HandleKeyboardInput(gameTime, parentActor);
            base.Update(gameTime, actor);
        }


        public virtual void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
        }

        public virtual void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
        }

        //add clone...
    }
}
