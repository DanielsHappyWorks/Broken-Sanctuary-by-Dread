using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class TexturedPrimitiveObject : PrimitiveObject, ICloneable
    {
        #region Variables
        private Texture2D texture;
        #endregion

        #region Properties
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

        public TexturedPrimitiveObject(string id, 
            ActorType actorType, Transform3D transform,
            BasicEffect effect, IVertexData vertexData, 
            Texture2D texture)
            : base(id, actorType, transform, effect, vertexData)
        {
            this.texture = texture;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Effect.Texture = texture;
            base.Draw(gameTime);
        }

        public new object Clone()
        {
            return new TexturedPrimitiveObject("clone - " + ID, //deep
               this.ActorType, //deep
               (Transform3D)this.Transform3D.Clone(), //deep
               this.Effect, //shallow - its ok if objects refer to the same effect
               this.VertexData, //shallow - its ok if objects refer to the same vertices
               this.texture); //shallow - its ok if objects refer to the same texture
        }
    }
}
