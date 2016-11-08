using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class BoxPrimitiveObject : DrawableGameComponent
    {
        private Vector3 position, rotation, scale;
        private BasicEffect effect;
        private Texture2D texture;
        private Main game;
        private VertexPositionColorTexture[] verts;
        private float tileRate;

        private BlendState blendState;
        private Color color;
        private RasterizerState rasterizerState; 
        private SamplerState samplerState;
        private float alpha;

        public BoxPrimitiveObject(Main game, Vector3 position, Vector3 rotation,
            Vector3 scale, BasicEffect effect, Texture2D texture, float tileRate, 
            Color color, RasterizerState rasterizerState,
            BlendState blendState, SamplerState samplerState, float alpha)
            : base(game)
        {
            this.game = game;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.effect = effect;
            this.texture = texture;

            this.tileRate = tileRate;
            this.color = color;
            this.rasterizerState = rasterizerState;
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.alpha = alpha;
        }
        public override void Initialize()
        {
            InitializeVertices();
            base.Initialize();
        }
        private void InitializeVertices()
        {
           //array of points
            this.verts = new VertexPositionColorTexture[36];

            float halfSideLength = 0.5f;

            Vector3 topLeftFront = new Vector3(-halfSideLength, halfSideLength, halfSideLength);
            Vector3 topLeftBack = new Vector3(-halfSideLength, halfSideLength, -halfSideLength);
            Vector3 topRightFront = new Vector3(halfSideLength, halfSideLength, halfSideLength);
            Vector3 topRightBack = new Vector3(halfSideLength, halfSideLength, -halfSideLength);

            Vector3 bottomLeftFront = new Vector3(-halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomLeftBack = new Vector3(-halfSideLength, -halfSideLength, -halfSideLength);
            Vector3 bottomRightFront = new Vector3(halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomRightBack = new Vector3(halfSideLength, -halfSideLength, -halfSideLength);

            //uv coordinates
            Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);


            //top - 1 polygon for the top
            this.verts[0] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            this.verts[1] = new VertexPositionColorTexture(topLeftBack, this.color, uvTopLeft);
            this.verts[2] = new VertexPositionColorTexture(topRightBack, this.color, uvTopRight);

            this.verts[3] = new VertexPositionColorTexture(topLeftFront, this.color, uvBottomLeft);
            this.verts[4] = new VertexPositionColorTexture(topRightBack, this.color, uvTopRight);
            this.verts[5] = new VertexPositionColorTexture(topRightFront, this.color, uvBottomRight);

            //front
            this.verts[6] = new VertexPositionColorTexture(topLeftFront, this.color, uvBottomLeft);
            this.verts[7] = new VertexPositionColorTexture(topRightFront, this.color, uvBottomRight);
            this.verts[8] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvTopLeft);

            this.verts[9] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvTopLeft);
            this.verts[10] = new VertexPositionColorTexture(topRightFront, this.color, uvBottomRight);
            this.verts[11] = new VertexPositionColorTexture(bottomRightFront, this.color, uvTopRight);

            //back
            this.verts[12] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);
            this.verts[13] = new VertexPositionColorTexture(topRightBack, this.color, uvTopRight);
            this.verts[14] = new VertexPositionColorTexture(topLeftBack, this.color, uvTopLeft);

            this.verts[15] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);
            this.verts[16] = new VertexPositionColorTexture(topLeftBack, this.color, uvTopLeft);
            this.verts[17] = new VertexPositionColorTexture(bottomLeftBack, this.color, uvBottomLeft);

            //left 
            this.verts[18] = new VertexPositionColorTexture(topLeftBack, this.color, uvTopLeft);
            this.verts[19] = new VertexPositionColorTexture(topLeftFront, this.color, uvTopRight);
            this.verts[20] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvBottomRight);

            this.verts[21] = new VertexPositionColorTexture(bottomLeftBack, this.color, uvBottomLeft);
            this.verts[22] = new VertexPositionColorTexture(topLeftBack, this.color, uvTopLeft);
            this.verts[23] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvBottomRight);

            //right
            this.verts[24] = new VertexPositionColorTexture(bottomRightFront, this.color, uvBottomLeft);
            this.verts[25] = new VertexPositionColorTexture(topRightFront, this.color, uvTopLeft);
            this.verts[26] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);

            this.verts[27] = new VertexPositionColorTexture(topRightFront, this.color, uvTopLeft);
            this.verts[28] = new VertexPositionColorTexture(topRightBack, this.color, uvTopRight);
            this.verts[29] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);

            //bottom
            this.verts[30] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvTopLeft);
            this.verts[31] = new VertexPositionColorTexture(bottomRightFront, this.color, uvTopRight);
            this.verts[32] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);

            this.verts[33] = new VertexPositionColorTexture(bottomLeftFront, this.color, uvTopLeft);
            this.verts[34] = new VertexPositionColorTexture(bottomRightBack, this.color, uvBottomRight);
            this.verts[35] = new VertexPositionColorTexture(bottomLeftBack, this.color, uvBottomLeft);

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

            this.effect.Texture = this.texture;

            //render front, back, or both faces of the primitive
            this.game.GraphicsDevice.RasterizerState = this.rasterizerState;
            //enable support for alpha blending i.e. semi-transparent objects
            this.game.GraphicsDevice.BlendState = this.blendState;
            this.effect.Alpha = this.alpha;

            this.effect.CurrentTechnique.Passes[0].Apply();

            this.effect.GraphicsDevice.DrawUserPrimitives
            <VertexPositionColorTexture>(PrimitiveType.TriangleList, this.verts, 0, 12);

            base.Draw(gameTime);
        }
    }
}
