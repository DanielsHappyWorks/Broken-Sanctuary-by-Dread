using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    //allows movement in any XZ direction (i.e. y is set to zero to prevent flight)
    public class FirstPersonController : UserInputController
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        public FirstPersonController(string id,
            ControllerType controllerType, Keys[] moveKeys,
            float moveSpeed, float strafeSpeed, float rotationSpeed)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed)
        {
        }

        public override void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
            Vector2 mouseDelta = game.MouseManager.GetDeltaFromCentre();
            mouseDelta *= gameTime.ElapsedGameTime.Milliseconds;
            mouseDelta *= this.RotationSpeed;

            parentActor.Transform3D.RotateBy(new Vector3(-mouseDelta, 0));

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

            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexStrafeLeft]))
            {
                parentActor.Transform3D.TranslateIncrement
                    += -gameTime.ElapsedGameTime.Milliseconds
                             * this.StrafeSpeed * parentActor.Transform3D.Right;
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexStrafeRight]))
            {
                parentActor.Transform3D.TranslateIncrement
                    += gameTime.ElapsedGameTime.Milliseconds
                             * this.StrafeSpeed * parentActor.Transform3D.Right;
            }

            //prevent movement up or down
          //  parentActor.Transform3D.TranslateIncrementY = 0;

            if (parentActor.Transform3D.TranslateIncrement != Vector3.Zero)
            {
                parentActor.Transform3D.TranslateBy(parentActor.Transform3D.TranslateIncrement);
                parentActor.Transform3D.TranslateIncrement = Vector3.Zero;
            }
        }

        //add clone...
    }
}