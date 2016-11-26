using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UIMouseObject : UITextureObject
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
        Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, bool isVisible)
            : this(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, 
                new Rectangle(0, 0, texture.Width, texture.Height),
                    new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), isVisible)
        {

        }

        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, Rectangle sourceRectangle, Vector2 origin, bool isVisible)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, sourceRectangle, origin, isVisible)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            this.Transform2D.Translation = game.MouseManager.Position;
            game.SpriteBatch.Draw(this.Texture, this.Transform2D.Translation, //bug - 22/4/16
                this.SourceRectangle, this.Color, this.Transform2D.Rotation, this.Origin, 
                    this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMouseObject(gameTime);
            DoMousePick(gameTime);
            base.Update(gameTime);
        }

        private void UpdateMouseObject(GameTime gameTime)
        {
            //move the texture for the mouse object to be where the mouse pointer is
            this.Transform2D.Translation = game.MouseManager.Position;
        }


        public virtual void DoMousePick(GameTime gameTime)
        {
            if (game.CameraManager.ActiveCamera != null)
            {
                //add code for mouse picking from the previous week....
            }
        }
    }
}
