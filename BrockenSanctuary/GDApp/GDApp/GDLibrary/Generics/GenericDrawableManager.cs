/*
Function: 		Provides core methods used by both UIManager and ObjectManager
Author: 		NMCG
Version:		1.0
Date Updated:	29/11/16
Bugs:			None
Fixes:			None
*/
using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class GenericDrawableManager : DrawableGameComponent
    {
        #region Variables
        private Main game;
        private string name;
        private GenericList<Actor> opaqueDrawList, transparentDrawList, removeList;
        private bool bPauseUpdate, bPauseDraw;
        #endregion

        #region Properties
        public new Main Game
        {
            get
            {
                return this.game;
            }
        }
        public bool PauseDraw
        {
            get
            {
                return this.bPauseDraw;
            }
            set
            {
                this.bPauseDraw = value;
            }
        }
        public bool PauseUpdate
        {
            get
            {
                return this.bPauseUpdate;
            }
            set
            {
                this.bPauseUpdate = value;
            }
        }
        public int OpaqueCount
        {
            get
            {
                return this.opaqueDrawList.Count;
            }
        }
        public int TransparentCount
        {
            get
            {
                return this.transparentDrawList.Count;
            }
        }
        protected GenericList<Actor> OpaqueDrawList
        {
            get
            {
                return this.opaqueDrawList;
            }
        }
        protected GenericList<Actor> TransparentDrawList
        {
            get
            {
                return this.transparentDrawList;
            }
        }
        #endregion

        //a manager that starts paused
        public GenericDrawableManager(Main game, string name, int initialSize)
            : this(game, name, initialSize, true, true)
        {

        }
        public GenericDrawableManager(Main game, string name,
            int initialSize, bool bPauseUpdate, bool bPauseDraw)
            : base(game)
        {
            this.name = name;
            this.game = game;
            this.opaqueDrawList = new GenericList<Actor>(initialSize);
            this.transparentDrawList = new GenericList<Actor>(initialSize);
            this.removeList = new GenericList<Actor>(initialSize);
            this.bPauseUpdate = bPauseUpdate;
            this.bPauseDraw = bPauseDraw;

            #region Event Handling
            //pause/unpause events
            this.game.EventDispatcher.MenuChanged += EventDispatcher_MenuChanged;

            //opacity change events
            this.game.EventDispatcher.OpacityChanged += EventDispatcher_OpacityChanged;
            #endregion
        }

        private void EventDispatcher_OpacityChanged(EventData eventData)
        {
            Actor actor = eventData.Sender as Actor;
            if(actor != null)
            {
                if(actor.GetAlpha() == 1) //was transparent but now opaque
                {
                    this.Remove(actor);
                    this.opaqueDrawList.Add(actor);
                }
                else //was opaque but now transparent
                {
                    this.Remove(actor);
                    this.transparentDrawList.Add(actor);
                }

            }
        }

        #region Event Handling
        //show/hide UI
        private void EventDispatcher_MenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnPlay)
            {
                this.bPauseUpdate = false;
                this.bPauseDraw = false;
            }
            else if (eventData.EventType == EventActionType.OnPause)
            {
                this.bPauseUpdate = true;
                this.bPauseDraw = true;
            }
        }
        #endregion

        public void Add(Actor actor)
        {
            if(actor.GetAlpha() == 1)
                this.opaqueDrawList.Add(actor);
            else
                this.transparentDrawList.Add(actor);
        }
        public void Remove(Actor actor)
        {
            this.removeList.Add(actor);
        }
        public int Remove(IFilter<Actor> filter)
        {
            List<Actor> resultList = null;

            resultList = this.opaqueDrawList.FindAll(filter);
            if ((resultList != null) && (resultList.Count != 0)) //the actor(s) were found in the opaque list
            {
                foreach (Actor actor in resultList)
                    this.removeList.Add(actor);
            }
            else //the actor(s) were found in the transparent list
            {
                resultList = this.transparentDrawList.FindAll(filter);

                if ((resultList != null) && (resultList.Count != 0))
                    foreach (Actor actor in resultList)
                        this.removeList.Add(actor);
            }

            return resultList != null ? resultList.Count : 0;
        }
        //batch remove on all objects that were requested to be removed
        protected virtual void ApplyRemove()
        {
            foreach (Actor actor in this.removeList)
            {
                if (actor.GetAlpha() == 1)
                    this.opaqueDrawList.Remove(actor);
                else
                    this.transparentDrawList.Remove(actor);
            }

            this.removeList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            ApplyRemove();
            if (!this.bPauseUpdate)
            {
                //update all your visible or invisible things opaque objects
                foreach (IActor actor in this.opaqueDrawList)
                {
                    //was the updated enum value set?
                    if (((actor as Actor).StatusType & StatusType.Updated) == StatusType.Updated)
                        actor.Update(gameTime);
                }

                //update all your visible or invisible things transparent objects
                foreach (IActor actor in this.transparentDrawList)
                {
                    //was the updated enum value set?
                    if (((actor as Actor).StatusType & StatusType.Updated) == StatusType.Updated)
                        actor.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
    }
}
