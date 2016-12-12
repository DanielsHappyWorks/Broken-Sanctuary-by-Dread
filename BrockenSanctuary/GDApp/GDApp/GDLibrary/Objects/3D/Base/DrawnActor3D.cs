using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class DrawnActor3D : Actor3D, ICloneable
    {
        #region Variables
        private Effect effect;
        private Color color;
        private float alpha;
        #endregion

        #region Properties
        public Effect Effect
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
                value = (value >= 0 && value <= 1) ? value : 1; //bounds check on value

                //notify the object manager to move the object from one draw list to the other based on new alpha
                if(value == 1 && this.alpha != 1) //going from opaque to transparent
                    EventDispatcher.Publish(new EventData(this.ID, this, EventActionType.OnAlpha, EventCategoryType.OpacityChange));
                else if(value != 1 && this.alpha == 1) //going from transparent to opaque
                    EventDispatcher.Publish(new EventData(this.ID, this, EventActionType.OnAlpha, EventCategoryType.OpacityChange));

                //assign the new alpha
                this.alpha = value;
            }
        }
        #endregion

        //used when we don't want to specify color and alpha
        public DrawnActor3D(string id, ActorType actorType,
         Transform3D transform, Effect effect)
            : this(id, actorType, transform, effect, 
            Color.White, 1, 
            StatusType.Drawn | StatusType.Updated) //when we bitwise OR we saw drawn AND updated
        {
        }

        //forward compatibility (since v3.4) for existing code with no StatusType
        public DrawnActor3D(string id, ActorType actorType,
           Transform3D transform, Effect effect, Color color, float alpha)
            : this(id, actorType, transform, effect, color, alpha, 
            StatusType.Drawn | StatusType.Updated) //when we bitwise OR we saw drawn AND updated

        {

        }

        //used by ZoneObjects
        public DrawnActor3D(string id, ActorType actorType,
           Transform3D transform, Color color, float alpha)
            : this(id, actorType, transform, null, color, alpha,
            StatusType.Drawn | StatusType.Updated) //when we bitwise OR we saw drawn AND updated
        {

        }

        public DrawnActor3D(string id, ActorType actorType,
            Transform3D transform, Effect effect, Color color, 
            float alpha, StatusType statusType)
            : base(id, actorType, transform, statusType)
        {
            this.effect = effect;
            this.color = color;
            this.alpha = alpha;
        }

        public override float GetAlpha()
        {
            return this.alpha;
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
