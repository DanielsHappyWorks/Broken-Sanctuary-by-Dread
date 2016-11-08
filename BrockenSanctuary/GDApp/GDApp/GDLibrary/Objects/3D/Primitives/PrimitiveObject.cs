using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class PrimitiveObject : DrawnActor3D, ICloneable
    {
        #region Variables
        private IVertexData vertexData;
        #endregion 

        #region Properties
        public IVertexData VertexData
        {
            get
            {
                return this.vertexData;
            }
            set
            {
                this.vertexData = value;
            }
        }
        #endregion

        public PrimitiveObject(string id, ActorType actorType,
            Transform3D transform,
                BasicEffect effect, IVertexData vertexData)
            : base(id, actorType, transform, effect)
        {
            this.vertexData = vertexData;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Effect.View = game.CameraManager.ActiveCamera.View;
            this.Effect.Projection = game.CameraManager.ActiveCamera.Projection;
            this.Effect.World = this.Transform3D.World;
            this.Effect.DiffuseColor = this.Color.ToVector3();
            this.Effect.Alpha = this.Alpha;

            this.Effect.CurrentTechnique.Passes[0].Apply();
            this.vertexData.Draw(gameTime, this.Effect);
        }

        public new object Clone()
        {
            return new PrimitiveObject("clone - " + ID, //deep
               this.ActorType, //deep
               (Transform3D)this.Transform3D.Clone(), //deep
               this.Effect, //shallow - its ok if objects refer to the same effect
               this.vertexData); //shallow - its ok if objects refer to the same vertices
        }
    }
}
