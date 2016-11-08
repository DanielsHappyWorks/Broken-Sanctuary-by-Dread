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
        private List<IActor> drawList;
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
            : this(game, name, 10)
        {
            this.game = game;
            this.bDebugMode = bDebugMode;
        }

        public ObjectManager(Main game, string name,
            int initialSize) : base(game)
        {
            this.name = name;
            this.drawList = new List<IActor>(initialSize);

            InitializeGraphicsStateObjects();
        }

        private void InitializeGraphicsStateObjects()
        {
            this.rasterizerState = new RasterizerState();
            this.rasterizerState.FillMode = FillMode.Solid;

            //set to None for transparent objects
            this.rasterizerState.CullMode = CullMode.None;
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

        public bool Remove(IActor actor)
        {
            return this.drawList.Remove(actor);
        }

        public bool Remove(IFilter<Actor> filter)
        {
            Actor actor = Find(filter);
            if (actor != null)
            {
                this.drawList.Remove(actor);
                return true;
            }

            return false;
        }
        public int RemoveAll(IFilter<Actor> filter)
        {
            Actor actor;
            int count = 0;
            for (int i = 0; i < this.drawList.Count; i++)
            {
                actor = this.drawList[i] as Actor;
                if (filter.Matches(actor))
                {
                    this.drawList.Remove(actor);
                    count++; //counts how many we remove
                    i--; //if we remove then decrement i to ensure we remove duplicate neighbour 
                }
            }
            return count;
        }

        public Actor Find(IFilter<Actor> filter)
        {
            Actor actor;
            for (int i = 0; i < this.drawList.Count; i++)
            {
                actor = this.drawList[i] as Actor;
                if (filter.Matches(actor))
                    return actor;
            }
            return null;
        }

        public List<Actor> FindAll(IFilter<Actor> filter)
        {
            List<Actor> outList = new List<Actor>();
            Actor actor;
            for (int i = 0; i < this.drawList.Count; i++)
            {
                actor = this.drawList[i] as Actor;
                if (filter.Matches(actor))
                    outList.Add(actor);
            }

            //if nothing found then return null, otherwise return list
            return outList.Count > 0 ? outList : null;
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.bPaused)
            {
                //update all your visible or invisible things
                foreach (IActor actor in this.drawList)
                {
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
                    actor.Draw(gameTime);
                    DebugDrawCollisionSkin(actor);
                }
            }

            base.Draw(gameTime);
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
