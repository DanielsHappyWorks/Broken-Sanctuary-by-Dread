using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class EffectParameters
    {
        #region Variables
        private Main game;
        private BasicEffect effect;
        private RasterizerState rasterizerState;
        private BlendState blendState;
        private SamplerState samplerState;
        #endregion 

        #region Properties
        public BasicEffect Effect
        {
            get
            {
                return this.effect;
            }
            set
            {
                this.effect = value;
            }
        }
        public RasterizerState RasterizerState
        {
            get
            {
                return this.rasterizerState;
            }
            set
            {
                this.rasterizerState = value;
            }
        }
        public BlendState BlendState
        {
            get
            {
                return this.blendState;
            }
            set
            {
                this.blendState = value;
            }
        }
        public SamplerState SamplerState
        {
            get
            {
                return this.samplerState;
            }
            set
            {
                this.samplerState = value;
            }
        }
        #endregion

        public EffectParameters(Main game, BasicEffect effect, RasterizerState rasterizerState, BlendState blendState, SamplerState samplerState)
        {
            this.game = game;
            this.effect = effect;
            this.rasterizerState = rasterizerState;
            this.blendState = blendState;
            this.samplerState = samplerState;
        }

        public virtual void Draw(Actor3D actor)
        {
            this.effect.View = this.game.CameraManager.ActiveCamera.View;
            this.effect.Projection = this.game.CameraManager.ActiveCamera.Projection;
            this.effect.World = actor.Transform3D.World;

            //render front, back, or both faces of the primitive
            this.game.GraphicsDevice.RasterizerState = this.rasterizerState;

            //enable support for alpha blending i.e. semi-transparent objects
            this.game.GraphicsDevice.BlendState = this.blendState;

            //smooth between pixels
            this.game.GraphicsDevice.SamplerStates[0] = this.samplerState;

            //when the draw call is invoked then apply the W, V, P and states specified above to the GFX card
            this.effect.CurrentTechnique.Passes[0].Apply();
        }
    }
}
