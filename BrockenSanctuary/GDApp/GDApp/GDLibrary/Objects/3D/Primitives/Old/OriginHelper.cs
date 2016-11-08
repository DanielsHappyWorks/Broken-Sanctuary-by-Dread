using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GDApp;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class OriginHelper : DrawableGameComponent
    {
        private Vector3 position, rotation, scale;
        private VertexPositionColor[] verts;
        private BasicEffect effect;
        private Main game;

        public OriginHelper(Main game, Vector3 position, 
            Vector3 rotation, Vector3 scale, BasicEffect effect)
            : base(game)
        {
            this.game = game;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.effect = effect;
        }

        public override void Initialize()
        {
            IntialiseVertices();
            base.Initialize();
        }

        private void IntialiseVertices()
        {
            this.verts = new VertexPositionColor[6];
            float halfLength = 0.5f;

            //x-axis
            this.verts[0]
                = new VertexPositionColor(new Vector3(-halfLength, 0, 0),
                    Color.Red);

            this.verts[1]
              = new VertexPositionColor(new Vector3(halfLength, 0, 0),
                  Color.Red);

            //y-axis
            this.verts[2]
              = new VertexPositionColor(new Vector3(0, halfLength, 0),
                  Color.Green);

            this.verts[3]
              = new VertexPositionColor(new Vector3(0, -halfLength, 0),
                  Color.Green);

            //z-axis
            this.verts[4]
              = new VertexPositionColor(new Vector3(0, 0, halfLength),
                  Color.Blue);

            this.verts[5]
              = new VertexPositionColor(new Vector3(0, 0, -halfLength),
                  Color.Blue);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.effect.View = this.game.CameraManager.ActiveCamera.View;
            this.effect.Projection = this.game.CameraManager.ActiveCamera.Projection;
            this.effect.World = Matrix.Identity
                * Matrix.CreateScale(this.scale)
                * Matrix.CreateRotationX(
                MathHelper.ToRadians(this.rotation.X))
                 * Matrix.CreateRotationY(
                MathHelper.ToRadians(this.rotation.Y))
                 * Matrix.CreateRotationZ(
                MathHelper.ToRadians(this.rotation.Z))
                * Matrix.CreateTranslation(this.position);

            this.effect.CurrentTechnique.Passes[0].Apply();

            this.effect.GraphicsDevice.DrawUserPrimitives
                <VertexPositionColor>(PrimitiveType.LineList,
                this.verts, 0, 3);

            base.Draw(gameTime);
        }
    }
}
