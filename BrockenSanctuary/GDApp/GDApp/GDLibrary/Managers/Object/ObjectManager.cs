/*
Function: 		Store, update, and draw all visible objects
Author: 		NMCG
Version:		1.0
Date Updated:	13/10/16
Bugs:			None
Fixes:			None
*/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ObjectManager : DrawableGameComponent
    {
        #region Variables
        private string name;
        private List<IActor> drawList, removeList;
        private RasterizerState rasterizerState;
        private bool bPaused;
        private bool bDebugMode;
        private Main game;
        #endregion

        #region Properties
        public bool IsDebugMode 
        { 
            get
            {
                return this.bDebugMode;
            }
            set
            {
                this.bDebugMode = value;
            }
        }
        public int Count
        {
            get
            {
                return this.drawList.Count;
            }
        }
        public IActor this[int index]
        {
            get
            {
                return this.drawList[index];
            }
        }
        public bool Paused //to do...
        {
            get
            {
                return this.bPaused;
            }
            set
            {
                this.bPaused = value;
            }
        }
        #endregion

        public ObjectManager(Main game, string name, bool bDebugMode)
            : this(game, name, 10, bDebugMode)
        {
        }
        public ObjectManager(Main game, string name,
            int initialSize, bool bDebugMode)
            : base(game)
        {
            this.name = name;
            this.game = game;
            this.bDebugMode = bDebugMode;
            this.drawList = new List<IActor>(initialSize);
            this.removeList = new List<IActor>(initialSize);
            InitializeGraphicsStateObjects();
  
            #region Event Handling
            this.game.EventDispatcher.MenuChanged //passing function pointer to delegate
                += new EventDispatcher.MenuEventHandler(EventDispatcher_MenuChanged);
            #endregion
        }

        void EventDispatcher_MenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnPlay)
                this.bPaused = false; //in game, UI
            else if (eventData.EventType == EventActionType.OnPause)
                this.bPaused = true; // in menu, no UI
        }








        private void InitializeGraphicsStateObjects()
        {
            this.rasterizerState = new RasterizerState();
            this.rasterizerState.FillMode = FillMode.Solid;

            //set to None for transparent objects
            this.rasterizerState.CullMode = CullMode.None;
        }
        private void SetGraphicsStateObjects()
        {
            //Remember this code from our initial aliasing problems with the Sky box?
            //enable anti-aliasing along the edges of the quad i.e. to remove jagged edges to the primitive
            this.Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            //set the appropriate state e.g. wireframe, cull none?
            this.Game.GraphicsDevice.RasterizerState = this.rasterizerState;

            //enable alpha blending for transparent objects i.e. trees
            this.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //disable to see what happens when we disable depth buffering - look at the boxes
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Add(IActor actor)
        {
            //unique? this.drawList.Contains(actor)
            this.drawList.Add(actor);
        }
        public void Add(List<IActor> actorList)
        {
            foreach (IActor actor in actorList)
                this.drawList.Add(actor);
        }
        public void Remove(IActor actor)
        {
            this.removeList.Add(actor);
        }
        public bool Remove(IFilter<IActor> filter)
        {
            IActor actor = Find(filter);
            if (actor != null)
            {
                this.removeList.Add(actor);
                return true;
            }
            return false;
        }
        public int RemoveAll(IFilter<IActor> filter)
        {
            int count = 0;
            List<IActor> list = FindAll(filter);
            foreach (IActor actor in list)
            {
                this.removeList.Add(actor);
                count++;
            }
            return count;
        }
        public IActor Find(IFilter<IActor> filter)
        {
            IActor actor;
            for (int i = 0; i < this.drawList.Count; i++)
            {
                actor = this.drawList[i] as Actor;
                if (filter.Matches(actor))
                    return actor;
            }
            return null;
        }
        public List<IActor> FindAll(IFilter<IActor> filter)
        {
            List<IActor> outList = new List<IActor>();
            IActor actor;
            for (int i = 0; i < this.drawList.Count; i++)
            {
                actor = this.drawList[i] as IActor;
                if (filter.Matches(actor))
                    outList.Add(actor);
            }

            //if nothing found then return null, otherwise return list
            return outList.Count > 0 ? outList : null;
        }   
        //batch remove on all objects that were requested to be removed
        private void ApplyRemove()
        {
            foreach (IActor actor in this.removeList)
                this.drawList.Remove(actor);

            this.removeList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            ApplyRemove();

            if (!this.bPaused)
            {
                //update all your visible or invisible things
                foreach (IActor actor in this.drawList)
                {
                    //was the updated enum value set?
                    if (((actor as Actor3D).StatusType & StatusType.Updated) == StatusType.Updated)
                        actor.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (!this.bPaused)
            {
                SetGraphicsStateObjects();
                foreach (IActor actor in this.drawList)
                {
                    //was the drawn enum value set?
                    if (((actor as Actor3D).StatusType & StatusType.Drawn) == StatusType.Drawn)
                    {
                        actor.Draw(gameTime);
                        DebugDrawCollisionSkin(actor);
                    }
                }
            }

            base.Draw(gameTime);
        }

        //debug method to draw collision skins for collidable objects and zone objects
        private void DebugDrawCollisionSkin(IActor actor)
        {
            if ((actor is CollidableObject) && (this.IsDebugMode))
            {                  
                CollidableObject collidableObject = actor as CollidableObject;
                this.game.PhysicsManager.DebugDrawer.DrawDebug(collidableObject.Body, collidableObject.Collision);
            }
        }
    }
}
