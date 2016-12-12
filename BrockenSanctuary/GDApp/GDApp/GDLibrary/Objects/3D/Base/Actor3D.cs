using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Actor3D : Actor, ICloneable
    {
        #region Variables
        private Transform3D transform;
        private List<IController> controllerList;
        #endregion

        #region Properties
        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }
        public Transform3D Transform3D
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        #endregion

        public Actor3D(string id, ActorType actorType,
                            Transform3D transform, StatusType statusType)
            : base(id, actorType, statusType)
        {

            this.transform = transform;
        }

        public void AttachController(IController controller)
        {
            if(this.controllerList == null)
                this.controllerList = new List<IController>();
            this.controllerList.Add(controller); //duplicates?
        }
        public bool DetachController(string id)
        {
            return false; //to do...
        }
        public bool DetachController(IController controller)
        {
            return false; //to do...
        }

        public override void Update(GameTime gameTime)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime, this); //you control me, update!
            }
            base.Update(gameTime);
        }

        public override Matrix GetWorldMatrix()
        {
            return this.transform.World;
        }

        public new object Clone()
        {
            return new Actor3D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform3D)this.transform.Clone(), //deep
                this.StatusType); //shallow
        }

        public override bool Remove()
        {
            //tag for garbage collection
            this.transform = null;
            if (this.controllerList != null)
            {
                this.controllerList.Clear();
                this.controllerList = null;
            }
            return base.Remove();
        }
    }
}
