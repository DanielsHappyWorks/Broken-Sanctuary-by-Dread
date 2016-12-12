using GDApp;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class Actor2D : Actor
    {
        #region Fields
        private Transform2D transform;
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
        public Transform2D Transform2D
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
        public Matrix World
        {
            get
            {
                return this.transform.World;
            }
        }
        #endregion

        public Actor2D(string id, ActorType actorType, StatusType statusType, Transform2D transform)
            : base(id, actorType, statusType)
        {
            this.transform = transform;
        }

        public void AttachController(IController controller)
        {
            if (this.controllerList == null)
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

        public override bool Remove()
        {
            //tag for garbage collection
            this.transform = null;
            return base.Remove();
        }

        //add clone...
    }
}
