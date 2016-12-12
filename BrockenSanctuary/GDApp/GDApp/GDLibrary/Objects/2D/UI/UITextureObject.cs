using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UITextureObject : DrawnActor2D
    {
        #region Fields
        private Texture2D texture;
        private Rectangle sourceRectangle, originalSourceRectangle;
        private Vector2 origin;
        #endregion

        #region Properties
        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }
        public Rectangle OriginalSourceRectangle
        {
            get
            {
                return this.originalSourceRectangle;
            }
        }
        public Rectangle SourceRectangle
        {
            get
            {
                return this.sourceRectangle;
            }
            set
            {
                this.sourceRectangle = value;
            }
        }
        public int SourceRectangleWidth
        {
            get
            {
                return this.sourceRectangle.Width;
            }
            set
            {
                this.sourceRectangle.Width = value;
            }
        }
        public int SourceRectangleHeight
        {
            get
            {
                return this.sourceRectangle.Height;
            }
            set
            {
                this.sourceRectangle.Height = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        #endregion

        public UITextureObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, 
            Rectangle sourceRectangle, Vector2 origin)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
            this.originalSourceRectangle = SourceRectangle;
            this.Origin = origin;
        }

        //draws texture using full source rectangle with origin in centre
        public UITextureObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
         Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture)
            : this(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, 
                new Rectangle(0, 0, texture.Width, texture.Height), 
                    new Vector2(texture.Width/2.0f, texture.Height/2.0f))
        {

        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Draw(this.texture,  this.Transform2D.Translation, 
                this.sourceRectangle, this.Color, 
                MathHelper.ToRadians(this.Transform2D.Rotation),
                this.Transform2D.Origin, this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }
    }
}
