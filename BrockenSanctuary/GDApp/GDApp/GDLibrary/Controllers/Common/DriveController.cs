using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    //creates a controller for drivable objects
    public class DriveController : UserInputController
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        public DriveController(string id,
            ControllerType controllerType, Keys[] moveKeys,
            float moveSpeed, float strafeSpeed, float rotationSpeed)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed)
        {
      
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            base.Update(gameTime, actor);
        }

        public override void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
           //to do...
        }

        public override void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveForward]))
            {
                parentActor.Transform3D.TranslateIncrement
                    = gameTime.ElapsedGameTime.Milliseconds
                             * this.MoveSpeed * parentActor.Transform3D.Look;
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveBackward]))
            {
                parentActor.Transform3D.TranslateIncrement
                    += -gameTime.ElapsedGameTime.Milliseconds
                             * this.MoveSpeed * parentActor.Transform3D.Look;
            }

            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateLeft]))
            {
                parentActor.Transform3D.RotateAroundYBy(gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed);
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateRight]))
            {
                parentActor.Transform3D.RotateAroundYBy(-gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed);
            }

            //prevent movement up or down
            //parentActor.Transform3D.TranslateIncrementY = 0;

            if (parentActor.Transform3D.TranslateIncrement != Vector3.Zero)
            {
                parentActor.Transform3D.TranslateBy(parentActor.Transform3D.TranslateIncrement);
                parentActor.Transform3D.TranslateIncrement = Vector3.Zero;
            }
        }

        //add clone...
    }
}