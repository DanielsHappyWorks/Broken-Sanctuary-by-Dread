using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class UIManager : DrawableGameComponent
    {
        #region Variables
        private Main game;
        private string name;
        private List<IActor> drawList, removeList;
        private bool bPaused;
        private bool bMouseVisible;
        #endregion

        #region Properties
        public bool MouseVisible
        {
            get
            {
                return this.bMouseVisible;
            }
            set
            {
                game.IsMouseVisible = this.bMouseVisible = value;
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

        public UIManager(Main game, string name, int initialSize, bool bMouseVisible)
            : base(game)
        {
            this.name = name;
            this.game = game;
            this.MouseVisible = bMouseVisible;
            this.drawList = new List<IActor>(initialSize);
            this.removeList = new List<IActor>(initialSize);
            this.bPaused = true;

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




        #region Event Handling
        //add any class specific event handling methods here   
        #endregion
        
        public void Add(IActor actor)
        {
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
                    if (((actor as Actor2D).StatusType & StatusType.Updated) == StatusType.Updated)
                        actor.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
       
        public override void Draw(GameTime gameTime)
        {    
            if (!this.bPaused)
            {
                this.game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                foreach (IActor actor in this.drawList)
                {
                    if (((actor as Actor2D).StatusType & StatusType.Drawn) == StatusType.Drawn)
                        actor.Draw(gameTime);
                }

                this.game.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
