using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private Vector3 translationOffset;
        private bool playFootsteps;
        private bool paused;
        private float count;
        private Vector2 outOfBoundsIncrementation;
        private Vector2 mouseDelta;
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
            this.playFootsteps = true;
            this.paused = false;
            this.count = 1;

            this.playerObject = new PlayerObject(this.ID + " - player object", ActorType.CollidableCamera, parentActor.Transform3D,
             null, Color.White, 1, null, null, this.MoveKeys, radius, height, accelerationRate, decelerationRate, translationOffset);
            playerObject.Enable(false, mass);
        }

        public override void HandleMouseInput(GameTime gameTime, Actor3D parentActor)
        {
            //code from the group that made Crime Rush
            if ((parentActor != null) && (parentActor != null))
            {
                Camera3D camera = parentActor as Camera3D;
                Vector2 mouseDelta = game.MouseManager.GetDeltaFromPosition(game.ScreenCentre);
                if (mouseDelta.X >= 480 || mouseDelta.X <= -480)
                {
                    game.MouseManager.SetPosition(new Vector2(game.ScreenCentre.X, Mouse.GetState().Y));
                }

                if (Mouse.GetState().Y >= 483)
                {
                    mouseDelta.Y = 99;
                    game.MouseManager.SetPosition(new Vector2(Mouse.GetState().X, 483f));
                }
                else if (Mouse.GetState().Y <= 285)
                {
                    mouseDelta.Y = -100;
                    game.MouseManager.SetPosition(new Vector2(Mouse.GetState().X, 285));
                }
                mouseDelta *= gameTime.ElapsedGameTime.Milliseconds;
                mouseDelta *= 0.047f;

                //this.Transform3D.RotateBy(new Vector3(-mouseDelta, 0));
                parentActor.Transform3D.RotateBy(new Vector3(-mouseDelta, 0));
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

                //footsteps
                if (game.KeyboardManager.IsFirstKeyPress(this.MoveKeys[AppData.IndexMoveForward])
                    || game.KeyboardManager.IsFirstKeyPress(this.MoveKeys[AppData.IndexMoveBackward])
                    || game.KeyboardManager.IsFirstKeyPress(this.MoveKeys[AppData.IndexRotateRight])
                    || game.KeyboardManager.IsFirstKeyPress(this.MoveKeys[AppData.IndexRotateLeft]))
                {
                    if (this.playFootsteps)
                    {
                        game.soundManager.PlayCue("footsteps");
                        playFootsteps = false;
                        paused = true;
                    }
                }
                else if (paused == true)
                {
                    if (!game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveForward])
                        && !game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveBackward])
                        && !game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateRight])
                        && !game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateLeft]))
                    {
                        game.soundManager.StopCue("footsteps", AudioStopOptions.Immediate);
                        this.playFootsteps = true;
                    }
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
                    //parentActor.Transform3D.RotateAroundYBy(this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
                }
                else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateRight]))
                {
                    Vector3 restrictedRight = parentActor.Transform3D.Right;
                    restrictedRight.Y = 0;
                    this.playerObject.CharacterBody.Velocity += restrictedRight * gameTime.ElapsedGameTime.Milliseconds * this.RotationSpeed;
                    //parentActor.Transform3D.RotateAroundYBy(-this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
                }
                else //decelerate to zero when not pressed
                {
                    this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
                }

                //update the camera position to reflect the collision skin position
                parentActor.Transform3D.Translation = this.playerObject.CharacterBody.Position;
            }

        }
    }
}