using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class DrawnActor3D : Actor3D, ICloneable
    {
        #region Variables
        private BasicEffect effect;
        private Color color;
        private float alpha;
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
        public float Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                this.alpha = value;
            }
        }
        #endregion

        //used when we don't want to specify color and alpha
        public DrawnActor3D(string id, ActorType actorType,
         Transform3D transform, BasicEffect effect)
            : this(id, actorType, transform, effect, Color.White, 1)
        {
        }

        public DrawnActor3D(string id, ActorType actorType,
            Transform3D transform, BasicEffect effect, Color color, float alpha)
            : base(id, actorType, transform)
        {
            this.effect = effect;
            this.color = color;
            this.alpha = alpha;
        }

        public new object Clone()
        {
            return new DrawnActor3D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform3D)this.Transform3D.Clone(), //deep
                this.effect, //shallow - its ok if objects refer to the same effect
                this.color, //deep
                this.alpha); //deep
        }
    }
}
