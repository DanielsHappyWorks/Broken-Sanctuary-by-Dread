using GDApp;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    /// <summary>
    /// Represents an area used for camera switching 
    /// </summary>
    public class CameraZoneObject : ZoneObject
    {
        #region Variables  
        #endregion

        #region Properties
        #endregion

        //no target specified e.g. we detect by object type not specific target address
        public CameraZoneObject(string id, ActorType actorType,
           Transform3D transform, bool isImpenetrable, EventParameters eventParameters)
            : base(id, actorType, transform, isImpenetrable, eventParameters)
        {

        }

        //we know address of the target object that will trigger the zone
        public CameraZoneObject(string id, ActorType actorType, Transform3D transform,
            bool isImpenetrable, EventParameters eventParameters, IActor targetActor)
            : base(id, actorType, transform, isImpenetrable, eventParameters, targetActor)
        {
         
        }

        public override bool HandleCollision(CollisionSkin collider, CollisionSkin collidee)
        {
            if(!((collidee.Owner.ExternalData as Actor3D).ID.Equals("ground")))
                System.Diagnostics.Debug.WriteLine((collidee.Owner.ExternalData as Actor3D).ID);
           
            //call the base method since it will return true/false based on whether the zone isImpenetrable
            return base.HandleCollision(collider, collidee);
        }
    }
}
