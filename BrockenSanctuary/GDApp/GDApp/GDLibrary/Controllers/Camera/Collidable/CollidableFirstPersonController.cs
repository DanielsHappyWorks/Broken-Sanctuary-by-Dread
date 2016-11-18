using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GDLibrary
{
    /// <summary>
    /// A collidable camera has a body and collision skin from a player object but it has no modeldata or texture
    /// </summary>
    public class CollidableFirstPersonController : FirstPersonController
    {
        #region Fields
        private PlayerObject playerObject;
        private bool bFirstTime;
        private float radius;
        private float height;
        private float accelerationRate;
        private float decelerationRate;
        private float mass;
        private bool isDirty;
        private Vector3 translationOffset;
        #endregion

        #region Properties
        #endregion


        public CollidableFirstPersonController(string id, ControllerType controllerType, 
                Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed, 
            float radius, float height, float accelerationRate, float decelerationRate, 
            float mass, Vector3 translationOffset, Actor3D parentActor)
            : base(id, controllerType, moveKeys, moveSpeed, strafeSpeed, rotationSpeed)
        {
            this.radius = radius;
            this.height = height;
            this.accelerationRate = accelerationRate;
            this.decelerationRate = decelerationRate;
            this.mass = mass;
            this.translationOffset = translationOffset;

            this.playerObject = new PlayerObject(this.ID + " - player object", ActorType.CollidableCamera, parentActor.Transform3D,
             null, Color.White, 1, null, null, this.MoveKeys, radius, height, accelerationRate, decelerationRate, translationOffset);
            playerObject.Enable(false, mass);
        }

        public override void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
            if ((parentActor != null) && (parentActor != null))
            {
                Camera3D camera = parentActor as Camera3D;
                Vector2 mouseDelta = game.MouseManager.GetDeltaFromPosition(camera.ViewportCentre);
                parentActor.Transform3D.RotateBy(new Vector3(-mouseDelta * gameTime.ElapsedGameTime.Milliseconds * 0.015f, 0));

                /*
                if (isDirty == true) { 
                    parentActor.Transform3D.RotateBy(new Vector3(-mouseDelta * gameTime.ElapsedGameTime.Milliseconds * 0.01f, 0));
                }
                else { 
                    game.MouseManager.SetPosition(new Vector2(game.Graphics.PreferredBackBufferWidth/2, game.Graphics.PreferredBackBufferWidth / 2));
                    isDirty = false;
                }*/
            }
        }

        public override void HandleKeyboardInput(GameTime gameTime, Actor3D parentActor)
        {
            if ((parentActor != null) && (parentActor != null))
            {
                //jump
                if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveJump]))
                {
                    this.playerObject.CharacterBody.DoJump(AppData.CameraJumpHeight);
                }
                //crouch
                else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveCrouch]))
                {
                    this.playerObject.CharacterBody.IsCrouching = !this.playerObject.CharacterBody.IsCrouching;
                }

                //forward/backward
                if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveForward]))
                {
                    Vector3 restrictedLook = parentActor.Transform3D.Look;
                    restrictedLook.Y = 0;
                    this.playerObject.CharacterBody.Velocity += restrictedLook * this.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveBackward]))
                {
                    Vector3 restrictedLook = parentActor.Transform3D.Look;
                    restrictedLook.Y = 0;
                    this.playerObject.CharacterBody.Velocity -= restrictedLook * this.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                }
                else //decelerate to zero when not pressed
                {
                    this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
                }

                //strafe left/right
                if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateLeft]))
                {
                    Vector3 restrictedRight = parentActor.Transform3D.Right;
                    restrictedRight.Y = 0;
                    this.playerObject.CharacterBody.Velocity -= restrictedRight * gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed;
                }
                else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateRight]))
                {
                    Vector3 restrictedRight = parentActor.Transform3D.Right;
                    restrictedRight.Y = 0;
                    this.playerObject.CharacterBody.Velocity += restrictedRight * gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed;
                }
               else //decelerate to zero when not pressed
                {
                    this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
                }

                //update the camera position to reflect the collision skin position
                parentActor.Transform3D.Translation = this.playerObject.CharacterBody.Position;
            }

        }


        //to do - clone, dispose

    }
}
