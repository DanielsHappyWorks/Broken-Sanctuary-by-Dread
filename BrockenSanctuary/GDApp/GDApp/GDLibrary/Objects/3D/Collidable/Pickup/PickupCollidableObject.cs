using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class PickupCollidableObject : CollidableObject
    {
        #region Variables
        private EventParameters eventParameters;
        #endregion

        #region Properties
        public EventParameters EventParameters
        {
            get
            {
                return this.eventParameters;
            }
            set
            {
                this.eventParameters = value;
            }
        }
        #endregion

        public PickupCollidableObject(string id, ActorType actorType, Transform3D transform, BasicEffect effect,
            Color color, float alpha, Texture2D texture, Model model, EventParameters eventParameters)
            : base(id, actorType, transform, effect, color, alpha, texture, model)
        {
            this.eventParameters = eventParameters;
        }

        //to do...clone, remove
    }
}
