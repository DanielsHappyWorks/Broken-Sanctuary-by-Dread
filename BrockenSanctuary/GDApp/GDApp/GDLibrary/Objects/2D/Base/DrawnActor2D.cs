using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class DrawnActor2D : Actor2D
    {
        #region Fields
        private Color color, originalColor;
        private float alpha, originalAlpha, layerDepth, originalLayerDepth;
        private SpriteEffects originalSpriteEffects, spriteEffects;
        private bool isVisible;
        #endregion

        #region Properties
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
            }

        }
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }
        public Color OriginalColor
        {
            get
            {
                return this.originalColor;
            }
            set
            {
                this.originalColor = value;
            }
        }

        public float Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                this.alpha = ((value >= 0) && (value <= 1))
                    ? value : 1;
            }
        }
        public float OriginalAlpha
        {
            get
            {
                return this.originalAlpha;
            }
            set
            {
                this.originalAlpha = ((value >= 0) && (value <= 1))
                    ? value : 1;
            }
        }
        public float LayerDepth
        {
            get
            {
                return this.layerDepth;
            }
            set
            {
                this.layerDepth = ((value >= 0) && (value <= 1))
                    ? value : 0;
            }
        }
        public float OriginalLayerDepth
        {
            get
            {
                return this.originalLayerDepth;
            }
            set
            {
                this.originalLayerDepth = ((value >= 0) && (value <= 1))
                    ? value : 0;
            }
        }
        public SpriteEffects SpriteEffects
        {
            get
            {
                return this.spriteEffects;
            }
            set
            {
                this.spriteEffects = value;
            }
        }
        public SpriteEffects OriginalSpriteEffects
        {
            get
            {
                return this.originalSpriteEffects;
            }
            set
            {
                this.originalSpriteEffects = value;
            }
        }
        #endregion

        public DrawnActor2D(string id, ActorType actorType, StatusType statusType, Transform2D transform, 
            Color color, SpriteEffects spriteEffects, float layerDepth)
            : base(id, actorType, statusType, transform)
        {
            this.color = color;
            this.spriteEffects = spriteEffects;
            this.LayerDepth = layerDepth;

            this.originalColor = Color;
            this.layerDepth = LayerDepth;
            this.spriteEffects = spriteEffects;
        }
    }
}
