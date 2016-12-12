using GDLibrary;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDApp
{
    public class HeroPlayerObject : AnimatedPlayerObject
    {
        public HeroPlayerObject(string id, ActorType actorType, Transform3D transform,
            Effect effect, Texture2D texture, Model model, Color color, float alpha,
            Keys[] moveKeys, float radius, float height, float accelerationRate, float decelerationRate, 
            string takeName, Vector3 translationOffset)
            : base(id, actorType, transform, effect, texture, model, color, alpha,
                    moveKeys, radius, height, accelerationRate, decelerationRate, 
                        takeName, translationOffset)
          {

          }

        protected override void HandleKeyboardInput(GameTime gameTime)
        {
            //jump
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveJump]))
            {
                this.CharacterBody.DoJump(AppData.CameraJumpHeight);
            }
            //crouch
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveCrouch]))
            {
                this.CharacterBody.IsCrouching = !this.CharacterBody.IsCrouching;
            }

            //forward/backward
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveForward]))
            {
                Vector3 restrictedLook = this.Transform3D.Look;
                restrictedLook.Y = 0;
                this.CharacterBody.Velocity += restrictedLook * AppData.PlayerMoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexMoveBackward]))
            {
                Vector3 restrictedLook = this.Transform3D.Look;
                restrictedLook.Y = 0;
                this.CharacterBody.Velocity -= restrictedLook * AppData.PlayerMoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }
            else //decelerate to zero when not pressed
            {
                this.CharacterBody.DesiredVelocity = Vector3.Zero;
            }

            //strafe left/right
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateLeft]))
            {
                this.Transform3D.RotateAroundYBy(AppData.PlayerRotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[AppData.IndexRotateRight]))
            {
                this.Transform3D.RotateAroundYBy(-AppData.PlayerRotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
            else //decelerate to zero when not pressed
            {
                this.CharacterBody.DesiredVelocity = Vector3.Zero;
            }

            //update the camera position to reflect the collision skin position
            this.Transform3D.Translation = this.CharacterBody.Position + this.TranslationOffset;
        }

        public override bool CollisionSkin_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            CollidableObject collidableObject = collider.Owner.ExternalData as CollidableObject;

            if(collidableObject.ActorType == GDLibrary.ActorType.Pickup)
            {
                int x = 0;
            }

            return true;
        }
    }
}
